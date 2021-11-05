// https://www.educba.com/bootstrap-sort-table/
// här använder jag metod 2 för att sortera tabellen
//
// direkt från Sedgewicks Algorithms: bubble-sort
//
// void bubble(itemType a[], int N) {
//   int i, j;
//   for ( i = N ; i >= 1 ; i--)
//      for ( j=2; j <= i; j++)
//         if( a[j-1] > a[j] )         är a[j-1] > a[j] flytta åt höger
//           swap(a, j-1, j);
//}
//      efter en växling kommer nästa steg i den inre iterationen återigen att
//      att jämföra samma ord med det till höger.  Växlingen av just det ordet avbryts när det som kommer efter
//      är större (i ordningen)
//      det ordet som visade sig vara större kommer ordet i stället att få avancera i listan.
//
// SÅ:
// start: Ulf Bengt Micke Wei Simon
// i j   0     1     2     3     4
//       Ulf   Bengt Micke Wei   Simon
// 5 1         ^                       Ulf o Bengt växlar
//       Bengt Ulf   Micke Wei   Simon
// 5 2               ^                 Ulf o Micke växlar
//       Bengt Micke Ulf   Wei   Simon
// 5 3                     ^           Ingen ändring men vi kan ha ett ord som är större ?
//       Bengt Micke Ulf   Wei   Simon
// 5 4                           ^     Wei o Simon växlar - ja, Wei ska vara efter Simon
//       Bengt Micke Ulf   Simon Wei
//
// 5 5   slut på inre loop och ett ord mindre att bry sig om - dekrement för antalet aktuella ord
//
//       Bengt Micke Ulf   Simon Wei
// 4 1         ^                       Ingen ändring
//       Bengt Micke Ulf   Simon Wei
// 4 2               ^                 Ingen ändring
//       Bengt Micke Ulf   Simon Wei
// 4 3                      ^          Ulf o Simon växlar
//       Bengt Micke Simon Ulf   Wei
//
// 4 4   slut på inre loop
//
//       Bengt Micke Ulf   Simon Wei
// 3 1         ^                       Ingen ändring
//       Bengt Micke Ulf   Simon Wei
// 3 2               ^                 Ingen ändring
//       Bengt Micke Ulf   Simon Wei
//
// 3.3   slut på inre loop
//
//       Bengt Micke Ulf   Simon Wei
// 2 1         ^                       Ingen ändring
//       Bengt Micke Ulf   Simon Wei
//
// 2.2   slut på inre loop
//
//       Bengt Micke Ulf   Simon Wei
//
// 1 1   slut på inre loop

/// <summary>
/// samma men annat villkor för beslut - vi vill knuffa det större framåt !
///
/// API: under vilken div finns uppräkningen
///      sorteringsterm : vilken av värdena (de har index 0 .... n) ska användas för sorteringen
/// </summary>
function sorteraTabellStigande(listansid, sorteringsterm) {
    var tabellen = document.getElementById( listansid);
    var posterna = tabellen.children; // varje rad i tabellen - htmlcollection
    var i = posterna.length;                                // antalet poster

    for (; i >= 1; i--)
	for (let j = 1; j < i; j++)
	    if (posterna.item(j - 1) != undefined && posterna.item(j) != undefined &&
		posterna.item(j - 1).children.item(sorteringsterm).innerHTML > posterna.item(j).children.item(sorteringsterm).innerHTML)
		tabellen.insertBefore(posterna.item(j), posterna.item(j - 1));
}

/// <summary>
/// samma men annat villkor för beslut - vi vill knuffa det minsta framför oss !
///
/// API: under vilken div finns uppräkningen
///      sorteringsterm : vilken av värdena (de har index 0 .... n) ska användas för sorteringen
/// </summary>
function sorteraTabellSjunkande( listansid, sorteringsterm) {
    var tabellen = document.getElementById( listansid);
    var posterna = tabellen.children;   // varje rad i tabellen
    var i = posterna.length;                                // antalet poster

    for (; i >= 1; i--)
	for (let j = 1; j < i; j++)
	    if (posterna.item(j - 1) != undefined && posterna.item(j) != undefined &&
		posterna.item(j - 1).children.item(sorteringsterm).innerHTML < posterna.item(j).children.item(sorteringsterm).innerHTML)
		tabellen.insertBefore(posterna.item(j), posterna.item(j - 1));  // post (j) framför posten i j-1
}

/// <summary>
/// begränsad - den sorterar enbart på namn eller bostadsort (sorteringsterm) i stigande ordning
/// </summary>
// function sorteraTabell( listansid, sorteringsterm) {
//     var sorteringsordning = "stigande";  // sorteringsordning

//     if (sorteringsordning == "stigande")
//	sorteraTabellStigande( listansid, sorteringsterm);
//     else if (sorteringsordning == "sjunkande")
//	sorteraTabellSjunkande( listansid, sorteringsterm);
// }
