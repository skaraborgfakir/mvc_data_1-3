// - Time-stamp: <2021-11-05 12:21:24 stefan>

//
// validering av input till för kort
//

/// <summary>
/// TODO verifiera att validering av nytt kort fungerar
///
/// aktiveras via knapptryck på 'Skapa kortet' (nya_aktiva.cshtml)
/// </summary>
$(document).ready(function () {
    $('#nya_aktiva').validate( {
	debug: true,
	onkeyup: true,
	rules: {
	}
    })
});
