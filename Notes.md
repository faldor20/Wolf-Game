---
title: Notes on coding work
---

# A list of small ideas and thoughts

## About ai behaviour

The Prey needs to be able to see. to achive this we can use a collider for its sight, but for amny interactions the prey will need to do somethign relating to the closest enemy or friend or member of the group.

##Sight

sight will be managed using a collider. 3 colliders will be used so as to optimize the process.

### Optimization

We could possibly use multiple colliers to reduce checking but weather or not this would be faster is queationable. i think 3 clliders, one very small, one medium and one large would be a good option.

##Safeness

the safeness of each entity could be shared and givent to the group then sorted. then when each elk wants to move twards a friendly it will move towards

i think moving twarsd entities with higher safeness is pointless i think moving twarsd the group center would be better. then once they are far enought awyay form the center they can be considered lost.
then they can start clumping up with safeness.

## staying in a group

To make the prey stay in a group we shall make the prey check if it is near

we could make the pray allways list sightly twards the center of the group but that may caus unecissary bumping

this still allows for prey to drift off in small groups away ffrom the group they are assigned to.

one way to make sure prey move twards the "main" group is by assigning each prey a "safeness" stat depending on how many prey are within its first collider.(wolfs could allso be included) Then prey could move preferentially towards other prey with higher safeness.

prey with safeness above a ceratin level could then not bother moving twards other prey.

prey will allso need to be able to change speed in response to certain things.

1. a slow genreal move speed
2. faster general move speed
3. a needing to rejoin the group speed
4. a badly needing to rejoin the group speed
5. a runnin away from the wolf speed

perhaps a time limit could be had for each speed and thena delay on the recharge for that speed after the time limit runs out

# system

##optimzation

runnig the system every frame is excedingly inneficient i could have it so that the prey aimed to do something and would simply esecute a do command untill any of a set of triggers ocured breaking its behaviour and returing to a genral decision state. theses triggers could incluse:

1. moved to desired location
2. regained proximity with other entity.
3. in sight of wolf.

this could be coupled with a delay of x time between any ai checks say 100 miliseconds
(these delays would have to be staggard to stop all updates occuring at once, this coukld be donewith a componant and a system taht runs at the beginning and assigns each prey a number representing its staggaring. this number would be modulo of it staggering)

They safness update can run a frame before the rest of the ai update cycle.

# ToDO

[] create a way for prey to be removed from the group managers array of prey

[] Create a prey Spawner

[] impliment min safeness and safeness to move at as a ratio of the total group size

[]impliment turning related to movement speed. so that it is rotation over distance not just rotation over time, that way the woulf actually has to turn around

[] make it so each prey only does one hysics overlap by either combining the information in safness upadter and prey move or storing information between them
[]figre out how to prevent prey overshooting their target
perhaps
[]better simulate behaviour by having the elk eventually bolt close ish to the wolf of the group is getting to far away.
[]impliment path finding to allow the elk to move around the wolf
