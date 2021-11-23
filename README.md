Inlämningsuppgifter till Lexicon i MVC delen

I huvudsak uppgifter med olika repo som datalager

Identity Server :
Uppgift 2:
  fler roller:
    superadministratör
      ensam om att kunna förändra vilka användare som har administratörsrollen
      bildas vid databassådd

  kodkrav:
    minst tre roller

Uppgift 1:
  registrering av konton för användare i systemet, som ska kunna :
    se inlagda kort i systemet
    skriva in nya personkort
    modifiera existerande kort
  vem som helst ska kunna lägga in en ny användare i systemet
  när de lägger in sig själva som användare, ska de mata in:
    namn (för- och efter-namn)
    födelsedatum
  ett inlagt konto ska kunna markera ett annat som administratör
    administratörer ska ha samma roller som vanliga användare
    de ska även kunna se, modifiera och ta bort kort i systemets tre kategorier:
      personkort
      städer
      länder
    rollen för vanliga användare ska inte ha tillgång till kontrollanterna för städer och länder

Entity Framework 6 - Databas-ramverk:
Uppgift 1:
   Hantera Inversion of Control med Dependency Injection (ympning)
   Ympning av :
      PeopleService med IPeopleRepo
      PeopleController med IPeopleService
      PeopleAjaxController med IPeopleService

MVC Data:
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
