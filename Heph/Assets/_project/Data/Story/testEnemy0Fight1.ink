-> main


=== main ===
Here is a line of dialogue
And now we must test our flirting skills, of course!

How do you wish to attempt to flirt? #choice_type:flirt_card
* [ThreatenWithGoodTime]
    -> mainThreatenGoodTimeResponse
* [SmoothlySuggestive]
    -> mainSmoothlySuggestiveResponse
* [GetUncomfortablyClose]
    -> mainUncomfortableCloseness
* [AccidentallyFlop]
    -> mainAccidentallyFlop


=== mainThreatenGoodTimeResponse == 
Heph raises their sword up to the chin of testEnemyOne
Forcing a slight raise of the head, and a meeting of eyelines
    -> EndOfCombat

=== mainSmoothlySuggestiveResponse ===
Heph very awkwardly suggests that testEnemyOne should meet with them for coffee
    -> EndOfCombat
    
=== mainUncomfortableCloseness ===
Oops, didnt really mean to do that.
Nerves are playing up.
    -> EndOfCombat
    
=== mainAccidentallyFlop ===
OMG, you are so nervous.
In fact, you begin to doubt it is even physically possible for you to feel the things you are currently feeling in regards to testEnemyOne
    -> EndOfCombat

=== EndOfCombat ===
The combat ends, and both fighters leave towards their exits

How do you wish to leave? #choice_type:button
* [FollowEnemyToTheirExit] Follow Enemy To Their Exit
    You follow testEnemyOne to their exit, how they respond, you will never know
    -> DONE
* [GoToYourOwnExit] Go to your own exits
    You go to your own exit, clearly leaving no option for further talking
    -> DONE

-> END
