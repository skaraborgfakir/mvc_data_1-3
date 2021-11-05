//
// - Time-stamp: <2021-11-05 14:12:50 stefan>
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
	ignore: ".ignore",
	rules: {
	    valtkortsid: {
		required: true,
		min: 0
	    }
	},
	messages: {
	},
	// JavaScript-funktionen som ska hantera submit (visa kort)
	submitHandler: function(form) {

	},
	// Något är fel i input
	invalidHandler: function(event, validator) {
	}
    });
});
