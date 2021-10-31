// - Time-stamp: <2021-10-31 12:17:42 stefan>

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
    // json-kodad lista
    const url_listan =         "https://localhost:5009/api/PeopleAjax/uppdateralistan";
    // uppgifter om ett specifikt kort
    const url_specifikt_kort = "https://localhost:5009/api/PeopleAjax/tagUppKortet";
    // kasta kortet
    const url_kasera_kort =    "https://localhost:5009/api/PeopleAjax/kaseraKortet";

    /// <summary>
    /// inklistring av tabellhuvud med början på tabellen
    /// </summary>
    $("#tabell_kartotek_ajax").append( "<div class=\"row\"> " +
				       "<div class=\"col-md-2 text-start sorting\" id=\"sorteraefternamn\">Namn</div>" +
				       "<div class=\"col-md-2 text-start sorting\" id=\"sorteraefterbostadsort\">Bostadsort</div>"+
				       "<div class=\"col-md-2 text-start\">Telefon</div>"+
				       "<div class=\"col-md-1 text-start\">Id</div>"+
				       "<div class=\"col-md-2\">Aktioner</div>" +
				       "</div>" +
				       "<div id=\"enumreringajax\">" +
				       "</div>");

    /// få in en aktuell vy direkt
    uppdateraVy();

    /// <summary>
    /// insättning av uppgifter om en viss person i uppräkningen
    /// </summary>
    function enumeration( index, item) {
	$("#enumreringajax").append( "<div class=\"row headline\" style=\"display:flex; flex-wrap: wrap;\" >" +
				     "<div class=\"col-md-2 text-start\">" + item.namn + "</div>" +
				     "<div class=\"col-md-2 text-start\">" + item.bostadsort + "</div>" +
				     "<div class=\"col-md-2 text-start\">" + item.telefonnummer + "</div>" +
				     "<div class=\"col-md-1 text-start\">" + item.id + "</div>" +
				     "<div class=\"col-md-2\"> " +
				     "<button type=\"button\" class=\"btn btn-danger\"  onClick=\"rensaUrKortet("   + item.id + ")\" >radering</button>" +
				     "<button type=\"button\" class=\"btn btn-primary\" onClick=\"modifieraKortet(" + item.id + ")\" >modifiera</button>" +
				     "</div>" +
				     "</div>");
    }

    /// <summary>
    /// aktiveras en gång efter inläsning av sidan för att få den första bilden
    /// kan sedan aktiveras via knapptryck i vyn (Uppdatera listan)
    /// </summary>
    function uppdateraVy() {
	$("#enumreringajax").empty();
	$.ajax({
	    url: url_listan,
	    type: 'GET',
	    datatype: 'json',
	    success: function(res) {
		let utdraget = Object( res );
		$.each( utdraget, function( index, item) { enumeration( index, item); });
	    }
	});
    }

    /// <summary>
    /// sortera listan enligt en term
    /// </summary>
    $('#sorteraefternamn').click( function( event) {
	event.preventDefault();

	$('#sorteraefternamn').addClass( "sorting_asc");
	$('#sorteraefterbostadsort').removeClass( "sorting_asc");

	sorteraTabellStigande( 'enumreringajax', 0);
    });

    /// <summary>
    /// sortera listan enligt en term
    /// </summary>
    $('#sorteraefterbostadsort').click( function(event) {
	event.preventDefault();

	$('#sorteraefternamn').removeClass( "sorting_asc");
	$('#sorteraefterbostadsort').addClass( "sorting_asc");

	sorteraTabellStigande( 'enumreringajax', 1);
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (Uppdatera listan)
    /// </summary>
    $('#uppdateralistan').click(function( event) {
	event.preventDefault();
	uppdateraVy();
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (visa kortet)
    /// </summary>
    $('#plockaframkortet').click(function( event) {
	event.preventDefault();
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (kasera kortet)
    /// </summary>
    $('#kaserakortet').click(function( event) {
	event.preventDefault();
    });
});
