//
// - Time-stamp: <2021-11-24 22:16:39 stefan>
//
// AJAX:funktionalitet för personvyn i kartoteket
//

"use strict";

const url_samtliga_kort            = "https://localhost:5003/api/PeopleAjax/uppdateralistan";    // json-kodad lista
const url_specifikt_kort           = "https://localhost:5003/api/PeopleAjax/tagframvisstkort";   // enbart visning av ett specifikt kort
const url_modifiera_specifikt_kort = "https://localhost:5003/api/PeopleAjax/modifieravisstkort"; // modifiering eller radering av ett specifikt kort
const url_kasera_kort              = "https://localhost:5003/api/PeopleAjax/kaserakortet";       // kasera kortet

//
// funktion för hämtning och visning av kort ur kartoteket
//
function uppdateraVy() {
    //
    // klistra in uppräkningen av korten vid #kartotekvyn
    // och ordna eventhantering för de olika knapparna inne i kortuppräkningen
    //
    $.ajax({
	method: "GET",
	url: url_samtliga_kort,
	success: function ( data, textStatus, jqXHR) {
	    $("#kartotekvyn").html( data);
	    //
	    // hantera tryck på någon av tre knapparna som finns i uppräkningen för varje kort
	    //     Visa
	    //     Modifiering
	    //     Raderingsknappen
	    //
	    // radering: då försvinner korter direkt
	    // modifiering: använd #visa modifiering_av_specifikt_kort
	    // och klistra där in Shared/modifiera_kort.cshtml
	    //
	    // samtliga varianterna sparar först undan kortets Id, det blir nödvändigt
	    // i DELETE:metoden därför $(this) halvvägs igenom blir ogiltig
	    //
	    $("button[name=kortvisning]").on("click", function(event) {
		var kortets_id = $(this).val();
		console.log( "kortvisning - Id: " + kortets_id  );
		$.ajax({
		    method: "GET",
		    url: url_specifikt_kort + '/' + $.param( { "id": kortets_id }),
		    success: function( data, textStatus, jqXHR) {
			// göm uppräkningen (#kartotekvyn) och klistra in dialogen vid #visning_av_specifikt_kort
			$("#kartotekvyn").hide();
			$("#visning_av_specifikt_kort").html(data);
			$("#visning_av_specifikt_kort").show();
		    }
		});
	    });
	    $("button[name=modifierakortet]").on("click", function(event) {
		var kortets_id = $(this).val();
		console.log( "modifierakortet - Id: " + kortets_id );
		//
		// TODO: utifrån Id, hämta vyn för modifiering (GET)
		// göm uppräkningen (#kartotekvyn) och klistra in dialogen vid #modifiering_av_specifikt_kort
		//
		$.ajax({
		    method: "POST",
		    url: url_modifiera_specifikt_kort + '/' + $.param( { "id": kortets_id }),
		    success: function( data, textStatus, jqXHR) {
			$("#kartotekvyn").hide();
			$("#modifiering_av_specifikt_kort").html( data);
			$("#modifiering_av_specifikt_kort").show();
		    }
		});
	    });
	    $("button[name=kortkasering]").on("click", function(event) {
		var kortets_id = $(this).val();
		console.log( "kortkasering - Id: " + kortets_id  );
		$.ajax({
		    method: "DELETE",
		    url: url_kasera_kort + '?' + $.param( { "id": kortets_id }),
		    success: function( data, textStatus, jqXHR) {
			console.log( "radering av kort fungerade med id : " + kortets_id );
			uppdateraVy();
		    },
		    error: function( data, textStatus, jqXHR) {
			// omöjligt, då har vi fel i koden någonstans
			if ( data.status == 404 ) {
			    console.log( "radering av kort fungerade inte därför att id : " + kortets_id + " är ogiltigt" );
			    $("#kartotekvyn").hide();
			    $("#modifiering_av_specifikt_kort").html( jqXHR);
			    $("#modifiering_av_specifikt_kort").show();
			} else
			{
			    console.log( "radering av kort fungerade inte därför att data : " + data + " och jqXHR : " + jqXHR);
			    $("#kartotekvyn").hide();
			    $("#modifiering_av_specifikt_kort").html( jqXHR);
			    $("#modifiering_av_specifikt_kort").show();
			}
		    }
		});
	    });

	    //
	    // fixa anropen till sorterings-funktion iom att tabellhuvudet finns först nu
	    //
	    $("#sorteraefternamn").on( "click", function(event) {
		sorteraefternamn()
	    });
	    $("#sorteraefterbostadsort").on( "click", function(event) {
		sorteraefterbostadsort()
	    });
	}
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
    $("#uppdateralistan").on( "click", function( event) {
	uppdateraVy();
	return false;
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (visa kortet) (ajaxbaserad_kortselektor.cshtml)
    /// </summary>
    $("#visakortet").on( "click", function( ) {
	var kortets_id = $("#valtkortsid").val();
	$.ajax({
	    method: "GET",
	    url : url_specifikt_kort + "/" + $.param( { "id": kortets_id }),
	    success: function( data, textStatus, jqXHR) {
		// göm uppräkningen (#kartotekvyn) och klistra in dialogen vid #visning_av_specifikt_kort
		$("#kartotekvyn").hide();
		$("#visning_av_specifikt_kort").html(data);
		$("#visning_av_specifikt_kort").show();
	    }
	});
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (modifierakortet) (ajaxbaserad_kortselektor.cshtml)
    /// </summary>
    $("#modifierakortet").on( "click", function() {
	var kortets_id = $("#valtkortsid").val();
	$.ajax({
	    method: "POST",
	    url: url_modifiera_specifikt_kort + "?" + $.param( { "id": kortets_id }),
	    success: function( data, textStatus, jqXHR) {
		$("#kartotekvyn").hide();
		$("#modifiering_av_specifikt_kort").html( data);
		$("#modifiering_av_specifikt_kort").show();
	    },
	    error: function( data, textStatus, jqXHR) {
	    }
	});
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (kasera kortet) (ajaxbaserad_kortselektor.cshtml)
    /// </summary>
    /// <see href="https://stackoverflow.com/questions/15088955/how-to-pass-data-in-the-ajax-delete-request-other-than-headers">JQuery bug</see>
    /// <see href="http://bugs.jquery.com/ticket/11586">bug i jQuery: använder man DELETE så klipps data-klumpen bort</see>
    $("#kaserakortet").on( "click", function() {
	var kortets_id = $("#valtkortsid").val();

	$.ajax({
	    method: "DELETE",
	    url: url_kasera_kort + "?" + $.param( { "id": kortets_id }),
	    success: function( data, textStatus, jqXHR) {
	    },
	    error: function( data, textStatus, jqXHR) {
	    }
	});
    });
});

/// <summary>
/// sortera listan efter namn, aktiveras via klick på kolumnhuvudet (#sorteraefternamn) i PeopleAjax/aktivlistan_tabellhuvud.cshtml
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

    $( "#personindex_div div #sorteraefterbostadsort" ).removeClass( "sorting_asc");
    $( "#personindex_div div #sorteraefterbostadsort" ).removeClass( "sorting_desc");

    if ( !$("#sorteraefternamn").hasClass( "sorting_asc") &&
	 !$("#sorteraefternamn").hasClass( "sorting_desc")) {
	//
	// ingen sortering är i kraft men sortera i stigande
	//
	$("#sorteraefternamn").addClass( "sorting_asc");

	sorteraTabellStigande( "enumreringajax", 0);
    } else {
	// om sorteringen är inställd att vara sjunkande, tag bort den
	$("#sorteraefternamn.sorting_desc").removeClass( "sorting_desc");

	if ( $("#sorteraefternamn").hasClass( "sorting_asc")) {
	    $("#sorteraefternamn").removeClass( "sorting_asc");
	    $("#sorteraefternamn").addClass( "sorting_desc");

	    sorteraTabellSjunkande( "enumreringajax", 0);
	}
    }
}

/// <summary>
/// sortera listan efter bostadsort, aktiveras via klick på kolumnhuvudet (#sorteraefterbostadsort) i PeopleAjax/aktivlistan_tabellhuvud.cshtml
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
    $( "#personindex_div div #sorteraefternamn.sorting_asc" ).removeClass( "sorting_asc");
    $( "#personindex_div div #sorteraefternamn.sorting_desc" ).removeClass( "sorting_desc");

    if (! $("#personindex_div div #sorteraefterbostadsort").hasClass( "sorting_asc") &&
	! $("#personindex_div div #sorteraefterbostadsort").hasClass( "sorting_desc")) {
	//
	// ingen sortering är i kraft men sortera i stigande
	//
	$("#personindex_div div #sorteraefterbostadsort").addClass( "sorting_asc");
	sorteraTabellStigande( "enumreringajax", 1);
    } else {
	// om sorteringen är inställd att vara sjunkande, tag bort den
	$("#personindex_div div #sorteraefterbostadsort.sorting_desc").removeClass( "sorting_desc");

	// om sorteringen är inställd att vara stigande, ersätt den
	// med sjunkande ordning och sortera om
	if ( $("#personindex_div div #sorteraefterbostadsort").hasClass( "sorting_asc")) {
	    $("#personindex_div div #sorteraefterbostadsort").removeClass( "sorting_asc");
	    $("#personindex_div div #sorteraefterbostadsort").addClass( "sorting_desc");

	    sorteraTabellSjunkande( "enumreringajax", 1);
	}
    }
}

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
