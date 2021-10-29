// - Time-stamp: <2021-10-29 14:00:09 stefan>

//
// validering av input till för kort
//

$(document).ready(function () {
    $('#nya_aktiva').validate( {
	debug: false,
	onkeyup: true,
	rules: {
	}
    })
});
