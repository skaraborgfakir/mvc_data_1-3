// - Time-stamp: <2021-10-29 14:01:04 stefan>

//
// validering för Shared/ajaxbaserad_kortselektor.cshtml
//
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
    const url = "https://localhost:5009/PeopleAjax/uppdateralistan";

    function test () {
	$('#kortvy').append( "2test");
    }

    $('#uppdateralistan').click(function( event) {
	test();
	event.preventDefault();
    });

});

// @*
// req.onreadystatechange = function() {
//     if (this.readyState==XMLHttpRequest.DONE &&
//	this.status==200) {
//	document.getElementsById('kortvy').innerHTML = "test;"
//     }
// }
// req.open( "GET", url);
// req.send();
// let req = new XMLHttpRequest();
// *@
