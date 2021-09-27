//
// bubble-sort
// void bubble(itemType a[], int N) {
//   int i, j;
//   for ( i = N ; i >= 1 ; i--)
//      for ( j=2; j <= i; j++)
//         if( a[j-1] > a[j] )         är a[j-1] > a[j] flytta åt höger
//           swap(a, j-1, j);
//}
//      efter en växling kommer nästa steg i den inre iterationen återigen att
//      att jämföra samma ord.  Växlingen av just det ordet avbryts när det som kommer efter
//      är mindre. OM detta ord är större än det som är till höger om det så börjar processen om igen med just det ordet
//    SÅ:
//   start: Ulf Bengt Micke Wei Simon
//  i j
//        Ulf Bengt Micke Wei Simon
//  5 2         ^                     Ulf o Bengt växlar
//        Bengt Ulf Micke Wei Simon
//  5 3               ^               Ulf o Micke växlar
//        Bengt Micke Ulf Wei Simon
//  5 4                    ^          Ingen ändring
//        Bengt Micke Ulf Wei Simon
//  5 5                         ^     Wei o Simon växlar
//  5 6                               j > i - yttre iteration
//        Bengt Micke Ulf Simon Wei
//  4 2           ^                    Ingen ändring
//        Bengt Micke Ulf Simon Wei
//  4 3               ^                Ingen ändring
//        Bengt Micke Ulf Simon Wei
//  4 4                     ^           Ulf o Simon växlar
//  4 5                               j > i - yttre iteration
//        Bengt Micke Simon Ulf Wei
//  3 2             ^      Ingen ändring
//        Bengt Micke Simon Ulf Wei
//  3 3             ^      Ingen ändring
//        Bengt Micke Simon Ulf Wei
//  3 4                               j > i - yttre iteration
//  2 2             ^      Ingen ändring
//
//  1 2
//
//  0 -                    slut på yttre iteration

function sorteraTabellStigande( sorteringskriteria)
{
    var tabellen = document.getElementById( "personindex");
    var posterna = tabellen.rows;   // varje rad i tabellen
    var i;   // antalet poster
    var j;
    var x, y;   // de två posterna som det ska beslutas om

    for ( i = posterna.length; i >= 1 ; i-- ) {
	j=1;
	for ( j = 1; j < i ; j++) {
	    x = posterna[j-1].getElementsByTagName("TD")[sorteringskriteria];
	    y = posterna[j].getElementsByTagName("TD")[sorteringskriteria];

	    if ( x!=undefined && y != undefined)
		if ( x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase() )
		    posterna[j].parentNode.insertBefore( posterna[j], posterna[j-1] );
	}
    }
}

//
// samma från början men man börjar i andra ändan
//
// function sorteraTabellSjunkande( sorteringskriteria) {
//     var tabellen = document.getElementById( "personindex");
//     var posterna = tabellen.rows;   // varje rad i tabellen
//     var i = posterna.length;
//     var j;
//     var x, y;   // de två posterna som det ska beslutas om

//     for ( ; i >= 1 ; i++ )
//	for ( j = 2; j <= i ; j--) {
//	    x = posterna[i].getElementsByTagName("TD")[sorteringskriteria];
//	    y = posterna[i+1].getElementsByTagName("TD")[sorteringskriteria];

//	    if () {
//		posterna[i].parentNode.insertBefore( posterna[i+1], posterna[i]);
//	    }
//	}
// }


function sorteraTabell( sorteringskriteria) {
    alert('sorteraTabell');

    var sorteringsordning = "stigande";  // sorteringsordning

    // if ( sorteringsordning == "stigande")
	sorteraTabellStigande( sorteringskriteria);
    // else if ( sorteringsordning == "sjunkande")
    //	sorteraTabellSjunkande( sorteringskriteria);
}

// function sorteraTabell( sorteringskriteria) {
//     alert('sorteraTabell');

//     // var tabellen = document.getElementById( "personindex"),
//     var tabellen = document.getElementById( "personindex");
//     var sorteringsordning = "stigande";  // sorteringsordning
//     var posterna;                   // varje rad i tabellen
//     var antalOmflyttningar = 0;     // hur många omkastningar
//     let i=0;                        // itereringsvariabel
//     var x, y;

//	posterna = document.getElementById( "personindex").rows;


//	i = 2;
//	while (( i < (posterna.length )) && ! felaktigsortering) {
//	    x = posterna[i].getElementsByTagName("TD")[sorteringskriteria];
//	    y = posterna[i+1].getElementsByTagName("TD")[sorteringskriteria];

//	    switch( sorteringsordning) {
//	    case "stigande":
//		if ( x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
//		    felaktigsortering = true;
//		}
//		break;
//		case "sjunkande":
//		if ( x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
//		    felaktigsortering = true;
//		}
//		break;
//	    }
//	    i++;
//	}
//	if ( felaktigsortering) {
//	    posterna[i].parentNode.insertBefore( posterna[i+1], posterna[i]);
//	    antalOmflyttningar++;
//	    omkastninggjord=true;
//	}
//     }
