Inlämningsuppgifter till Lexicon i MVC delen

I huvudsak uppgifter med olika repo som datalager


Uppgift 3:
Feature-branch:
  - AJAX
      partial view ska hämta data från service-modulen gradvis
  - JavaScript för att modifiera listan så att reload efter exv ändrad sållning inte behövs
  - Ett formulär som om Id anges ger några uppgifter (namn, bostadsort, telefon)
  - Använd samma formulär för att radera
  - XML-baserad dokumentation av koden och doxygen för att få ut LaTeX (och i förlängning PDF)

Sedan tidigare:

Från uppgift 2:
Partial View som itererar igenom listan från Person-service:modellen används
Hela listan är gjord som ett träd av div

Den här versionen har:
  sortering av listan med ett eget javascript (bubbelsort)
  markering av vilka delar i tabellhuvudet som kan användas för sortering
  markering av sorteringsriktning

den använder bootstrap 5.1

TCP-portar för olika versioner:
  5002 (http),5003 (https) MVC Data uppgift 3 AJAX
