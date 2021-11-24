//
// - Time-stamp: <2021-11-24 14:46:43 stefan>
//
// sidan har följande organisation (delvyer) i People/Index.cshtml
// <html>  (Shared/Mallen.cshtml)
//  <head
//  <body>
//    <header>
//    </header>
//    <div class="container">
//      <main role="main"
//        <div class="text-center" (People/Index.cshtml)
//          <h3   Ett kartotek över aktiva
//          </h3>
//          <div>
//              <div class="container"                        (partial Shared/filtrering/dialog.cshtml)
//                <h4
//                </h4
//                <form
//                  <fieldset
//                  <div class="row mb-1                      (partial Shared/personkortvy_modellbaserad/filtreringstermer.cshtml)
//                    <label class="kortuppgifter ... Namn
//                    <div
//                      <input  />
//                    </div
//                  </div
//                  <div class="row mb-1
//                    <label class="kortuppgifter ... Bostadsort
//                    <div
//                      <input  />
//                    </div
//                  </div                                     (/partial Shared/personkortvy_modellbaserad/filtreringstermer.cshtml)
//                  </fieldset
//                </form
//              </div                                         (partial Shared/filtrering/dialog.cshtml)
//            <hr>
//                                    (partial Shared/inskrivning_av_nytt_kort/dialog.cshtml)
//            <hr>
//                                    (partial Shared/ajaxbaserad_kartotekvy)
//          </div>
//        </div class="text-center" (People/Index.cshtml)
//      </main                   (Shared/Mallen.cshtml)
//    </div>
//    <footer>
//    </footer>
//      skript etc
//  </body>
// </html>
//      main
//       div class="text-center"   (People/Index.cshtml)
//         h3 ett kartotek...
//         div
//           div
//             div id=filtrering/dialog
//
//

'use strict';

const url_samtliga_kort            = "https://localhost:5003/api/PeopleAjax/uppdateralistan";    // json-kodad lista
const url_specifikt_kort           = "https://localhost:5003/api/PeopleAjax/tagframvisstkort";   // uppgifter om ett specifikt kort
const url_modifiera_specifikt_kort = "https://localhost:5003/api/PeopleAjax/modifieravisstkort"; // modifiering av ett specifikt kort
const url_kasera_kort              = "https://localhost:5003/api/PeopleAjax/kaserakortet";       // kasera kortet

//
// funktion för hämtning och visning av kort ur kartoteket
//
function uppdateraVy() {
    //
    // klistra in uppräkningen av korten vid #kartotekvyn
    // och ordna eventhantering för de olika knapparna i kortuppräkningen
    //
    $("#kartotekvyn").load( url_samtliga_kort, function() {
	//
	// hantera tryck på någon av de två radering- och modifierings-knapparna i kortuppräkningen som finns för varje kort
	//
	// radering: då försvinner korter direkt
	// modifiering: använd #visa modifiering_av_specifikt_kort
	// och klistra där in Shared/modifiera_kort.cshtml
	//
	$("button[name=modifierakortet]").on("click", function(event) {
	    //
	    // TODO: utifrån Id, hämta vyn för modifiering (GET)
	    // göm uppräkningen (#kartotekvyn) och klistra in dialogen vid #modifiering_av_specifikt_kort
	    //
	    $.ajax({
		url: url_modifiera_specifikt_kort + '/' + $(this).val(),
		method: 'GET',
		success: function( data, textStatus, jqXHR) {
		    $("#kartotekvyn").hide();
		    $("#visning_av_specifikt_kort").hide();
		    $("#modifiering_av_specifikt_kort").html( data);
		    $("#modifiering_av_specifikt_kort").show();
		}
	    });
	});

	$("button[name=kortkasering]").on("click", function(event) {
	    $.ajax({
		url: url_kasera_kort + '?' + $.param( { "id": $(this).val() }),
		method: 'DELETE',
		success: function(res) {
		    console.log( "radering av kort med Id: " + $(this).val() + " fungerade" );
		}
	    })
	});
    });

    //
    // om vyn för ett specifikt kort syns, göm den
    //
    $("#kartotekvyn").show();
    $("#visning_av_specifikt_kort").hide();
    $("#modifiering_av_specifikt_kort").hide();
}

$(document).ready(function() {
    /// <summary>
    /// aktiveras en gång efter inläsning av sidan
    /// används för att få den första bilden
    /// som sedan kan aktiveras via knapptryck i vyn (Uppdatera listan)
    /// </summary>

    /// få in en aktuell vy direkt
    uppdateraVy();

    /// <summary>
    /// aktiveras via knapptryck på 'Uppdatera listan' (ajaxbaserad_kortselektor.cshtml)
    /// </summary>
    $("#uppdateralistan").click(function( event) {
	event.preventDefault();
	uppdateraVy();

	return false;
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (visa kortet) (ajaxbaserad_kortselektor.cshtml)
    /// </summary>
    $('#plockaframkortet').click(function( ) {
	var id = document.getElementById("valtkortsid").value;


	$("#ajaxvy_kartotek").load( url_specifikt_kort + '?' + $.param( { "id": id } ),
				    function() {
				    }
				  );
	// $.ajax({
	//     url: url_specifikt_kort,
	//     data: { "id": id },
	//     success: function( result ){
	//	$("#ajaxvy_kartotek").html( result );
	//     }
	// });

	// $("#tabell_kartotek_ajax").load( url: url_specifikt_kort,
	//				 data: { "id": id }
	//			       );

	// $.ajax({
	//     url: url_specifikt_kort,
	//     type: 'GET',
	//     datatype: 'json',
	//     success: function(res) {
	//	let utdraget = Object( res );
	//	$.each( utdraget, function( index, item) { enumeration( index, item); });
	//     }
	// });
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (kasera kortet) (ajaxbaserad_kortselektor.cshtml)
    /// </summary>
    /// <see href="https://stackoverflow.com/questions/15088955/how-to-pass-data-in-the-ajax-delete-request-other-than-headers">JQuery bug</see>
    /// <see href="http://bugs.jquery.com/ticket/11586">bug i jQuery: använder man DELETE så klipps data-klumpen bort</see>
    $('#kaserakortet').click(function( event) {
	// event.preventDefault();
	// document.getElementById("valtkortsid").value  till url_kasera_kort
	// hämta valtkortsid från skrollern och skicka vidare till kaseraspecifiktkort
	var id = document.getElementById("valtkortsid").value;

	$.ajax({ type:  "DELETE",
		 url:    url_kasera_kort + '?' + $.param( { "id": id }),
		 contentType: "application/json; charset=utf-8"
	       }
	      );
    });
});

/// <summary>
/// sortera listan efter namn, aktiveras via onClick i aktivlistan.cshtml
///
/// se till att markeringarna i tabellhuvudet är korrekta
///
/// koden börjar alltid med att sortera i stigande ordning
/// därefter med vidare tryck i tabellhuvudet i sjunkande ordning för att vid nästa
/// tryck nolla sorteringen (då blir sorteringen den som repo:koden använder)
///
/// sorteraTabellStigande/sorteraTabellSjunkande sorterar listan  om den får id för tabell och indexet för den
/// kolumn som ska användas för sorteringen
/// </summary>
function sorteraefternamn() {
    //	event.preventDefault();

    $( "#sorteraefterbostadsort" ).removeClass( "sorting_asc");
    $( "#sorteraefterbostadsort" ).removeClass( "sorting_desc");

    if ( !$('#sorteraefternamn').hasClass( "sorting_asc") &&
	 !$('#sorteraefternamn').hasClass( "sorting_desc")) {
	//
	// ingen sortering är i kraft men sortera i stigande
	//
	$('#sorteraefternamn').addClass( "sorting_asc");

	sorteraTabellStigande( 'enumreringajax', 0);
    } else {
	// om sorteringen är inställd att vara sjunkande, tag bort den
	$('#sorteraefternamn.sorting_desc').removeClass( "sorting_desc");

	if ( $('#sorteraefternamn').hasClass( "sorting_asc")) {
	    $('#sorteraefternamn').removeClass( "sorting_asc");
	    $('#sorteraefternamn').addClass( "sorting_desc");

	    sorteraTabellSjunkande( 'enumreringajax', 0);
	}
    }
}

/// <summary>
/// sortera listan efter bostadsort, aktiveras via onClick i aktivlistan.cshtml
///
/// se till att markeringarna i tabellhuvudet är korrekta
///
/// koden börjar alltid med sortera i stigande ordning
/// därefter med vidare tryck i tabellhuvudet i sjunkande ordning för att vid nästa
/// tryck nolla sorteringen (då kan sorteringen bli den som repo:koden använder)
///
/// sorteraTabellStigande/sorteraTabellSjunkande sorterar listan om den får id för tabell och indexet för den
/// kolumn som ska användas för sorteringen
/// </summary>
function sorteraefterbostadsort () {
    //	event.preventDefault();

    $( "#sorteraefternamn.sorting_asc" ).removeClass( "sorting_asc");
    $( "#sorteraefternamn.sorting_desc" ).removeClass( "sorting_desc");

    if (! $('#sorteraefterbostadsort').hasClass( "sorting_asc") &&
	! $('#sorteraefterbostadsort').hasClass( "sorting_desc")) {
	//
	// ingen sortering är i kraft men sortera i stigande
	//
	$('#sorteraefterbostadsort').addClass( "sorting_asc");
	sorteraTabellStigande( 'enumreringajax', 1);
    } else {
	// om sorteringen är inställd att vara sjunkande, tag bort den
	$('#sorteraefterbostadsort.sorting_desc').removeClass( "sorting_desc");

	// om sorteringen är inställd att vara stigande, ersätt den
	// med sjunkande ordning och sortera om
	if ( $('#sorteraefterbostadsort').hasClass( "sorting_asc")) {
	    $('#sorteraefterbostadsort').removeClass( "sorting_asc");
	    $('#sorteraefterbostadsort').addClass( "sorting_desc");

	    sorteraTabellSjunkande( 'enumreringajax', 1);
	}
    }
}

/// <summary>
/// aktion i själva kortuppräkningen
///
/// aktiveras via knapptryck i listvyn (radering bland aktionerna)
/// </summary>
// function kaseraspecifiktkort( Id) {
//     //
//     // TODO: utifrån scrollern, radering av rätt kort
//     //

//     //	$.ajax({
//     //	    url: url_kasera_kort,
//     //	    type: 'POST',
//     //	    success: function(res) {
//     //		let utdraget = Object( res );
//     //	    }
//     //	});
// }

/// <summary>
/// aktion i själva kortuppräkningen
///
/// aktiveras via knapptryck i listvyn (modifiering)
/// </summary>
// function modifieraspecifiktkort( Id) {
//     //
//     // TODO: utifrån scrollern, modifiering av rätt kort
//     //
//     // $.ajax({
//     //     url: url_kasera_kort,
//     //     type: 'GET',
//     //     datatype: 'json',
//     //     success: function(res) {
//     //	let utdraget = Object( res );
//     //	/// $.each( utdraget, function( index, item) { enumeration( index, item); });
//     //     }
//     // });
// }

/// <summary>
/// Vilket ID är lägst ?
///
/// används för validering
/// </summary>
// function lägstId() {
//     // iterera igenom personlistan för att hitta högst kort:ID
// }

/// <summary>
/// Vilket ID är högst ?
///
/// används för validering
/// </summary>
// function högstId() {
//     // iterera igenom personlistan för att hitta högst kort:ID
// }

/// <summary>
/// Finns det något kort som har ett visst ID ?
///
/// används för validering
/// </summary>
// function finnsKortId( int it) {
//     // iterera igenom personlistan för se om något kort har ett visst ID
// }
