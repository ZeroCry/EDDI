﻿# Templating with EDDI

EDDI's speech responder uses Cottle for templating.  Cottle has a number of great features, including:

    * Ability to set and update variables, including arrays
    * Loops
    * Conditionals
    * Subroutines

Information on how to write Cottle templates is available at http://r3c.github.io/cottle/#toc-2, and EDDI's default templates use a lot of the functions available.

## State Variables

Cottle does not retain state between templates, but EDDI provides a way of doing this with state variables.  State variables are provided to each Cottle template, and templates can set state variables that will be made available in future templates.

State variables are available for individual templates in the 'state' object.  Note that state variables are not persistent, and the state is empty whenever EDDI restarts.  Also, because EDDI responders run asynchronously and concurrently there is no guarantee that, for example, the speech responder for an event will finish before the VoiceAttack responder for an event starts (or vice versa).

## Context

EDDI uses the idea of context to attempt to keep track of what it is talking about.  This can enhance the experience when used with VoiceAttack by allowing repetition and more detailed information to be provided.

## EDDI Functions

In addition to the basic Cottle features EDDI has a number of features that provide added functionality and specific information for Elite: Dangerous.  Details of these functions are as follows:

### BodyDetails()

This function will provide full information for a body given its name.

BodyDetails() takes a single mandatory argument of the name of the body for which you want more information.  If the body is not in the current system then it can be provided with a second parameter of the name of the system.

Common usage of this is to provide further information about a body, for example:

    {set body to BodyDetails("Earth", "Sol")}
    Earth is {body.distancefromstar} light years from the system's main star.

### CargoDetails()

This function will provide full information for a cargo, carried in the commander's hold.

CargoDetails() takes one mandatory argument, of two possible forms. 
- The first form, a commodity name of the cargo. If the commodity is not in the hold, a 'null' is returned.
- The second form, a mission ID associated with the cargo, as haulage. If the mission ID is not associated with haulage, a 'null' is returned.

Common usage of this is to provide further information about a particular cargo, for example:

    {set cargo to CargoDetails("Tea")}
    {if cargo && cargo.total > 0: You have {cargo.total} tonne{if cargo.total != 1: s} of {cargo.name} in your cargo hold.}

or for a mission-related event,

    {set cargo to CargoDetails(event.missionid)}
    {if cargo: {cargo.total} tonne{if cargo.total != 1: s} of {cargo.name} is in your hold for this mission.}

### CombatRatingDetails()

This function will provide full information for a combat rating given its name.

CombatRatingDetails() takes a single argument of the combat rating for which you want more information.

Common usage of this is to provide further information about your rating, for example:

    You have been promoted {CombatRatingDetails("Expert").rank} times.

### CommodityMarketDetails()

This function will provide full information for a commodity, including information that is specific to a market, given the commodity name.

CommodityMarketDetails() takes one mandatory argument and two optional arguments. 
- The first argument, the name of the commodity for which you want more information, is mandatory.
- The second argument, the name of the station to reference for market data, is optional. If not given then EDDI will default to the current station (if the current station is not set and no station is specified then this shall return empty). 
- The third argument, the name of the system to reference for market data, is optional. If not given then EDDI will default to the current star system (if the specified station cannot be found within the current star system then EDDI shall return empty).

Common usage of this is to provide further information about a commodity, for example:

    {set marketcommodity to CommodityMarketDetails("Pesticides", "Chelbin Service Station", "Wolf 397")}
    {marketcommodity.name} is selling for {marketcommodity.sellprice} with a current market demand of {marketcommodity.demand} units.

### Distance()

This function will provide the distance (in light years) between two systems.

Distance() takes either two or six arguments.
- Two argurments, the names of the two systems.
- Six arguments, the x, y, z coordinates of the two systems.

Examples of each usage:

    {set distance to Distance("Sol", "Betelgeuse")}
    {set distance to Distance(from.x, from.y, from.z, to.x, to.y, to.z)}

### EconomyDetails()

This function will provide full information for an economy given its name.

EconomyDetails() takes a single argument of the economy for which you want more information.

At current this does not have a lot of use as the economy object only contains its name, but expect it to be expanded in future.

### Emphasize()

This function allows you to give emphasis to specific words (to the extent supported by the voice you are using - your mileage may vary). This function uses SSML tags.

Emphasize() takes one mandatory argument: the text to speak with emphasis. If no secondary argument is specified, it shall default to a strong emphasis.
Emphasize() also takes one optional argument: the degreee of emphasis to place on the text (legal values for the degree of emphasis include "strong", "moderate", "none" and "reduced").

Common usage of this is to provide a more human-sounding reading of text by allowing the application of emphasis:

   That is a {Emphasize('beautiful', 'strong')} ship you have there.

### EmpireRatingDetails()

This function will provide full information for an empire rating given its name.

EmpireRatingDetails() takes a single argument of the empire rating for which you want more information.

Common usage of this is to provide further information about your rating, for example:

    You have been promoted {EmpireRatingDetails("Lord").rank} times.

### ExplorationRatingDetails()

This function will provide full information for an exploration rating given its name.

ExplorationRatingDetails() takes a single argument of the exploration rating for which you want more information.

Common usage of this is to provide further information about your rating, for example:

    You have been promoted {ExplorationRatingDetails("Surveyor").rank} times.

### FederationRatingDetails()

This function will provide full information for an federation rating given its name.

FederationRatingDetails() takes a single argument of the Federation rating for which you want more information.

Common usage of this is to provide further information about your rating, for example:

    You have been promoted {FederationRatingDetails("Post Commander").rank} times.

### GovernmentDetails()

This function will provide full information for a government given its name.

GovernmentDetails() takes a single argument of the government for which you want more information.

At current this does not have a lot of use as the government object only contains its name, but expect it to be expanded in future.

### HaulageDetails()

This function will provide 'haulage' information for a mission-related cargo. See the 'haulage' object for variable details.

HaulageDetails() takes one mandatory argument, a mission ID associated with the haulage. If the mission ID is not associated with haulage, a 'null' is returned.

Common usage of this is to provide further information about a particular mission haulage, for example:

    {set haulage to HaulageDetails(event.missionid)}
    {if haulage && haulage.deleivered > 0:
        {set total to haulage.amount + haulage.deleivered}
	    {haulage.type} mission to the cargo depot is {round(haulage.delivered / total * 100, 0)} percent complete.
    }

### Humanise()

This function will turn its argument into a more human number, for example turning 31245 in to "just over thirty thousand".

Humanise() takes one argument: the number to humanise.

Common usage of this is to provide human-sounding numbers when speaking rather than saying every digit, for example:

   You have {Humanise(cmdr.credits)} credits.

### ICAO()

This function will turn its argument into an ICAO spoken value, for example "NCC" becomes "November Charlie Charlie".

ICAO() takes one argument: the value to turn in to ICAO.

Common usage of this is to provide clear callsigns and idents for ships, for example:

   Ship ident is {ICAO(ship.ident)}.

### List()

This function will return a humanised list of items from an array (e.g. this, that, and the other thing).

List() takes a single argument, the array variable with items you want listed.

Common usage is to convert an array to a list, for example:

    {set systemsrepaired to ['the hull', 'the cockpit', 'corroded systems']}
    The limpet has repaired {List(systemsrepaired)}.

### Log()

This function will write the supplied text to EDDI's log.

Log() takes a single argument of the string to log.

### MaterialDetails()

This function will provide full information for a material given its name.

MaterialDetails() takes a single argument of the material for which you want more information.

Common usage of this is to provide further information about a material, for example:

    Iron is a {MaterialDetails("Iron").rarity.name} material.

### MissionDetails()

This function will provide full information for a mission given its mission ID.

MissionDetails() takes a single argument of the mission ID for which you want more information.

Common usage of this is to provide detailed information about a previously accepted mission, for example:

    {set mission to MissionDetails(event.missionid)}

### OneOf()

This function will take one of the arguments available to it, picking randomly.

OneOf() takes as many arguments are you want to give it.

Common usage of this is to provide variation to spoken text, for example:

    You have {OneOf("docked", "finished docking", "completed docking procedures")}.

### Occasionally()

This function will take its argument 1/*n*th of the time, the rest of time discarding it.

Occasionally() takes two arguments: n, and the text argument.

Common usage of this is to provide additional text that is said now and again but would become irritating if said all the time, for example:

   Boost engaged.  {Occasionally(7, "Hold on to something.")}

Note that Occasionally() works on random numbers rather than counters, so in the above example the additional text will not show up every 7th time you boost but will show up on average 1/7 of the times that you boost.

### P()

This function will attempt to provide phonetic pronunciation for the supplied text. This function uses SSML tags.

P() takes a single argument of the string for which to alter the pronunciation.

Common usage of this is to wrap the names of planets, powers, ships etc., for example:

    You are in the {P(system.name)} system.

### Pause()

This function will pause the speech for a given amount of time. This function uses SSML tags.

Pause() takes one argument: the number of milliseconds to pause.

Common usage of this is to allow speech to sync up with in-game sounds, for example to wait for a known response to a phrase before continuing, for example:

    Hello.  {Pause(2000)} Yes.

### Play()

This function will play an audio file as supplied in the argument. This function uses SSML tags.

Play() takes one argument: the path to the file to play.  This file must be a '.wav' file.  Any backslashes for path separators must be escaped, so '\\' must be written as '\\\\'

Common usage of this is to provide a pre-recorded custom audio file rather than use EDDI's text-to-speech, for example:

    {Play('C:\\Users\\CmdrMcDonald\\Desktop\\Warning.wav')}

### RouteDetails()

This function will produce a destination/route for valid mission destinations, dependent on the following 'routetype':

  * 'expiring' Destination of your next expiring mission.
  * 'farthest' Mission destination farthest from your current location.
  * 'most' Nearest system with the most missions.
  * 'nearest' Mission destination nearest to your current location.
  * 'route' 'Traveling Salesman' (RNNA) route for all active missions.
  * 'source' Mission destination to nearest 'cargo source'.
  * 'update' Next mission destination, once all missions in current system are completed.

Upon success of the query, a 'Missions route' event is triggered, providing a following event data:

  * 'routetype' Type of route query (see above).
  * 'destination' Destination system.
  * 'route' "_" Delimited missions systems list. Results dependent on route type.
    * 'most' Other systems with most number of missions.
    * 'route' Missions route list
    * 'source' list of source systems
  * 'count' Count of missions, systems, or expiry seconds, depending on route type.
    * 'expiring' Expiry seconds.
    * 'farthest' Missions in the system.
    * 'most' Number of most missions.
    * 'nearest' Missions in the system.
    * 'route' Systems in the route.
    * 'source' Number of source systems.
    * 'update' Remaining systems in the route.
  * 'routedistance' Remaining distance of the missions route, if applicable.
  * 'missionids' Mission ID(s) associated with the destination system.

Common usage of this is to provide destination/route details, dependent on the 'routetype', for example:

    {set system to RouteDetails("nearest")}

### SecondsSince()

This function will provide the number of seconds since a given timestamp.

SecondsSince() takes a single argument of a UNIX timestamp.

Common usage of this is to check how long it has been since a given time, for example:

    Station data is {SecondsSince(station.updatedat) / 3600} hours old.

### SecurityLevelDetails()

This function will provide full information for a security level given its name.

SecurityLevelDetails() takes a single argument of the security level for which you want more information.

At current this does not have a lot of use as the security level object only contains its name, but expect it to be expanded in future.

### SetState()

This function will set a session state value.  The value will be available as a property of the 'state' object in future templates within the same EDDI session.

SetState takes two arguments: the name of the state value to set, and its value.  The name of the state value will be converted to lower-case and spaces changed to underscores.  The value must be either a boolean, a number, or a string; other values will be ignored.

Common usage of this is to keep track of the cumulative or persistent information within a session, for example:

    {SetState("distance_travelled_today", state.distance_travelled_today + event.distance)}

### ShipCallsign()

This function will provide your ship's callsign in the same way that Elite provides it (i.e. manufacturer followed by first three letters of your commander name).

ShipCallsign() takes an optional ship ID for which to provide the callsign. If no argument is supplied then it provides the callsign for your current ship.

This will only work if EDDI is connected to the Frontier API.

### ShipDetails()

This function will provide full information for a ship given its name.

ShipDetails() takes a single argument of the model of the ship for which you want more information.

Common usage of this is to provide further information about a ship, for example:

    The Vulture is made by {ShipDetails("Vulture").manufacturer}.

### ShipName()

This function will provide the name of your ship.

If you have set up a phonetic name for your ship it will return that, otherwise if you have set up a name for your ship it will return that. The phonetic name uses SSML tags.

ShipName() takes an optional ship ID for which to provide the name. If no argument is supplied then it provides the name for your current ship.

If you have not set up a name for your ship it will just return "your ship".

### Spacialise()

This function will add spaces between letters in a string & convert to uppercase, in order to allow letters in a string to be pronounced individually.

Spacialise() takes one argument: the number to Spacialise.

Common usage of this is to provide a more human-sounding reading of a string of letters that are not a part of known word:

   Star luminosity class: {Spacialise(event.luminosityclass)}.

### SpeechPitch()

This function allows you to dynamically adjust the pitch of the spoken speech. This function uses SSML tags.

SpeechPitch() takes two mandatory arguments: the text to speak and the pitch at which to speak it (legal values for the pitch include "x-low", "low", "medium", "high", "x-high", "default", as well as percentage values like "-20%" or "+10%").

Common usage of this is to provide a more human-sounding reading of text with variation in the speech pitch:

   {SpeechPitch('Ok, who added helium to the life support unit?', 'high')}
   {Pause(1000)}
   {SpeechPitch('Countering with sodium hexa-flouride.', 'x-low')}
   Equilibrium restored.

### SpeechRate()

This function allows you to dynamically adjust the rate of the spoken speech. This function uses SSML tags.

SpeechRate() takes two mandatory arguments: the text to speak and the speech rate at which to speak it (legal values for the speech rate include "x-slow", "slow", "medium", "fast", "x-fast", "default", as well as percentage values like "-20%" or "+20%").

Common usage of this is to provide a more human-sounding reading of text with variation in the speech rate:

   {SpeechRate('The quick brown fox', 'x-slow')}
   {SpeechRate('jumped over the lazy dog', 'fast')}.

### SpeechVolume()

This function allows you to dynamically adjust the volume of the spoken speech. This function uses SSML tags.

##### Please take care with decibel values. If accidentally you blow out your speakers, that's totally on you. 
SpeechRate() takes two mandatory arguments: the text to speak and the valume at which to speak it (legal values for the speech volume include "silent", "x-soft", "soft", "medium", "loud", "x-loud", "default", as well as relative decibel values like "-6dB").
A value of "+0dB" means no change of volume, "+6dB" means approximately twice the current amplitude, "-6dB" means approximately half the current amplitude.

Common usage of this is to provide a more human-sounding reading of text with variation in speech volume:

   {SpeechVolume('The quick brown fox', 'loud')}
   {SpeechVolume('jumped over the lazy dog', 'x-soft')}.

### StartsWithVowel()

This function returns true or false depending on whether the first letter in a string is a vowel.

StartsWithVowel() takes one argument: the string that may or may not start with a vowel.

Common usage of this is to select the word that should proceed the string (e.g. **a** Adaptive Encryptors Capture vs **an** Adaptive Encryptors Capture).
   
   {if StartsWithVowel(event.name): an |else: a } {event.name}

### StateDetails()

This function will provide full information for a state given its name.

StateDetails() takes a single argument of the state for which you want more information.

At current this does not have a lot of use as the state object only contains its name, but expect it to be expanded in future.

### StationDetails()

This function will provide full information for a station given its name and optional system.

StationDetails() takes a single mandatory argument of the name of the station for which you want more information.  If the station is not in the current system then it can be provided with a second parameter of the name of the system.

Common usage of this is to provide further information about a station, for example:

    {set station to StationDetails("Jameson Memorial", "Shinrarta Dezhra")}
    Jameson Memorial is {station.distancefromstar} light years from the system's main star.

### SuperpowerDetails()

This function will provide full information for a superpower given its name.

SuperpowerDetails() takes a single argument of the superpower for which you want more information.

At current this does not have a lot of use as the superpower object only contains its name, but expect it to be expanded in future.

### SystemDetails()

This function will provide full information for a star system given its name.

SystemDetails() takes a single argument of the star system for which you want more information.

Common usage of this is to provide further information about a star system, for example:

    Sol has {len(SystemDetails("Sol").bodies)} bodies.

### TradeRatingDetails()

This function will provide full information for a trade rating given its name.

TradeRatingDetails() takes a single argument of the trade rating for which you want more information.

Common usage of this is to provide further information about your rating, for example:

    You have been promoted {TradeRatingDetails("Peddler").rank} times.

### Transmit() 
 
This function allows you to add a radio effect to speech. 
 
Transmit() takes one argument: the text to speak. For example: 
 
{Transmit("{ShipName()} returning from orbit.")} 

### Voice()

This function allows you to include a different voice in your script than then one currently selected. This function uses SSML tags.

Voice() takes two mandatory arguments: the text to speak and the voice to speak it (legal values for the voice shall match one of the voices listed by EDDI's `Text-to-Speech` tab. This is case-sensitive."). For Example:

{Voice("Now I can speak", "Microsoft Zira Desktop")}
{Voice("And I can listen", "Microsoft David Desktop")}
