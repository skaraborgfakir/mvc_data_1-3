//
// - Time-stamp: <2021-11-04 16:20:50 stefan>
//

//
// validering för Shared/ajaxbaserad_kortselektor.cshtml
//

'use strict';

$(document).ready(function () {
    $('#specifiktKort').validate( {
	debug: false,
	onkeyup: true,
	rules: {
	}
    });
});

//
// funktioner för hämtning och visning av kort ur kartoteket
//
$(document).ready(function() {
    const url_samtliga_kort =  "https://localhost:5009/api/PeopleAjax/uppdateralistan"; // json-kodad lista
    const url_specifikt_kort = "https://localhost:5009/api/PeopleAjax/tagUppKortet";    // uppgifter om ett specifikt kort
    const url_kasera_kort =    "https://localhost:5009/api/PeopleAjax/kaseraKortet";    // kasera kortet

    /// <summary>
    /// inklistring av tabellhuvud med början på tabellen
    /// </summary>
    // $("#tabell_kartotek_ajax").append( "<div class=\"row\"> " +
    //				       "<div class=\"col-md-2 text-start sorting\" id=\"sorteraefternamn\">Namn</div>" +
    //				       "<div class=\"col-md-2 text-start sorting\" id=\"sorteraefterbostadsort\">Bostadsort</div>"+
    //				       "<div class=\"col-md-2 text-start\">Telefon</div>"+
    //				       "<div class=\"col-md-1 text-start\">Id</div>"+
    //				       "<div class=\"col-md-2\">Aktioner</div>" +
    //				       "</div>" +
    //				       "<div id=\"enumreringajax\">" +
    //				       "</div>");

    /// få in en aktuell vy direkt
    uppdateraVy();

    /// <summary>
    /// insättning av uppgifter om en viss person i uppräkningen
    /// </summary>
    // function enumeration( index, item) {
    //	$("#enumreringajax").append( "<div class=\"row headline\" style=\"display:flex; flex-wrap: wrap;\" >" +
    //				     "<div class=\"col-md-2 text-start\">" + item.namn + "</div>" +
    //				     "<div class=\"col-md-2 text-start\">" + item.bostadsort + "</div>" +
    //				     "<div class=\"col-md-2 text-start\">" + item.telefonnummer + "</div>" +
    //				     "<div class=\"col-md-1 text-start\">" + item.id + "</div>" +
    //				     "<div class=\"col-md-2\"> " +
    //				     "<button type=\"button\" class=\"btn btn-danger\"  onClick=\"rensaUrKortet("   + item.id + ")\" >radering</button>" +
    //				     "<button type=\"button\" class=\"btn btn-primary\" onClick=\"modifieraspecifiktkort (" + item.id + ")\" >modifiera</button>" +
    //				     "</div>" +
    //				     "</div>");
    // }

    /// <summary>
    /// aktiveras en gång efter inläsning av sidan för att få den första bilden
    /// kan sedan aktiveras via knapptryck i vyn (Uppdatera listan)
    /// </summary>
    function uppdateraVy() {
	//$("#enumreringajax").empty();  // töm ur listan helt och bygg upp den på nytt

	$("#tabell_kartotek_ajax").load( url_samtliga_kort);

	// $.ajax({
	//     url: url_samtliga_kort,
	//     type: 'GET',
	//     success: function(res) {
	//	let utdraget = Object( res );
	//	$("tabell_kartotek_ajax").append(utdraget);
	//     }
	// });
    }

    /// <summary>
    /// sortera listan enligt en term
    /// se till att markeringarna i tabellhuvudet är korrekta
    ///
    /// koden börjar alltid med sortera i sjunkande ordning
    /// därefter med vidare tryck i tabellhuvudet i stigande ordning för att vid nästa
    /// tryck nolla sorteringen
    ///
    /// sorteraTabellStigande/sorteraTabellSjunkande sorterar listan  om den får id för tabell och indexet för den
    /// kolumn som ska användas för sorteringen
    /// </summary>
    $('#sorteraefternamn').click( function( event) {
	event.preventDefault();

	$('#sorteraefterbostadsort.sorting_asc').removeClass( "sorting_asc");
	$('#sorteraefterbostadsort.sorting_desc').removeClass( "sorting_desc");

	if ( !$('#sorteraefternamn').hasClass( "sorting_asc") &&
	     !$('#sorteraefternamn').hasClass( "sorting_desc")) {
	    $('#sorteraefternamn').addClass( "sorting_desc");
	} else {
	    $('#sorteraefternamn.sorting_asc').removeClass( "sorting_asc");
	    if ( $('#sorteraefternamn').hasClass( "sorting_desc")) {
		$('#sorteraefternamn').removeClass( "sorting_desc");
		$('#sorteraefternamn').addClass( "sorting_asc");
	    }
	}

	sorteraTabellStigande( 'enumreringajax', 0);
    });

    /// <summary>
    /// sortera listan enligt en term
    /// </summary>
    $('#sorteraefterbostadsort').click( function(event) {
	event.preventDefault();

	$('#sorteraefternamn.sorting_asc').removeClass( "sorting_asc");
	$('#sorteraefternamn.sorting_desc').removeClass( "sorting_desc");

	if (! $('#sorteraefterbostadsort').hasClass( "sorting_asc") &&
	    ! $('#sorteraefterbostadsort').hasClass( "sorting_desc")) {
	    $('#sorteraefterbostadsort').addClass( "sorting_desc");
	} else {
	    $('#sorteraefterbostadsort.sorting_asc').removeClass( "sorting_asc");

	    if ( $('#sorteraefterbostadsort').hasClass( "sorting_desc")) {
		$('#sorteraefterbostadsort').removeClass( "sorting_desc");
		$('#sorteraefterbostadsort').addClass( "sorting_asc");
	    }
	}

	sorteraTabellStigande( 'enumreringajax', 1);
    });

    /// <summary>
    /// aktiveras via knapptryck av någon av 'Uppdatera listan', 'Visa kort' eller 'Raderar valt kort'
    /// </summary>
    $('#uppdateralistan').click(function( event) {
	event.preventDefault();
	uppdateraVy();
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (kasera kortet)
    /// </summary>
    $('#kaserakortet').click(function( event) {
	event.preventDefault();
	// hämta valtkortsid från skrollern och skicka vidare till kaseraspecifiktkort
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (visa kortet)
    /// </summary>
    $('#plockaframkortet').click(function( event) {
	event.preventDefault();
	$.ajax({
	    url: url_specifikt_kort,
	    type: 'GET',
	    datatype: 'json',
	    success: function(res) {
		let utdraget = Object( res );
		/// $.each( utdraget, function( index, item) { enumeration( index, item); });
	    }
	});
    });

    /// <summary>
    /// aktionerna i själva kortuppräkningen
    ///
    /// aktiveras via knapptryck i listvyn (radering bland aktionerna)
    /// </summary>
    function kaseraspecifiktkort( Id) {
	$.ajax({
	    url: url_kasera_kort,
	    type: 'POST',
	    success: function(res) {
		let utdraget = Object( res );
	    }
	});

    }

    /// <summary>
    /// aktionerna i själva kortuppräkningen
    ///
    /// aktiveras via knapptryck i listvyn (radering bland aktionerna)
    /// </summary>
    function modifieraspecifiktkort( Id) {
	// $.ajax({
	//     url: url_kasera_kort,
	//     type: 'GET',
	//     datatype: 'json',
	//     success: function(res) {
	//	let utdraget = Object( res );
	//	/// $.each( utdraget, function( index, item) { enumeration( index, item); });
	//     }
	// });
    }
});
