//
// - Time-stamp: <2021-11-05 12:26:47 stefan>
//

//
// validering för Shared/ajaxbaserad_kortselektor.cshtml
//

// TODO verifiera att validering av kortväljaren (scroll) i ajaxbaserad_kortselektor.cshtml
/// <summary>
/// kontrollera att skrollern i dialogen i ajaxbaserad_kortselektor fungerar
/// </summary>
$(document).ready(function () {
    $('#specifiktKort').validate( {
	debug: true,
	onkeyup: true,
	rules: {
	}
    });
});
