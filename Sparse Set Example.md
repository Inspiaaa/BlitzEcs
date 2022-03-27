
sparse = 0 0 0 0 0 0
dense  = 0 0 0 0 0 0
n = 0


---
Adding component to entity 3

sparse = 0 0 0 0 0 0
dense  = 3 0 0 0 0 0
n = 1


---
Adding component to entity 2

sparse = 0 0 1 0 0 0
dense  = 3 2 0 0 0 0
n = 2


---
Adding component to entity 4

sparse = 0 0 1 0 2 0
dense  = 3 2 4 0 0 0
n = 3


---
sparse = 0 0 1 0 2 0
dense  = 3 2 4 0 0 0
n = 3

Does entity 2 have a component?
-> sparse[2] => 1
-> 1 < n
-> dense[1]  => 2
-> Entity 2 has a component.


---
Deleting entity 2

Before:
sparse = 0 0 1 0 2 0
dense  = 3 2 4 0 0 0
n = 3

After:
sparse = 0 0 1 0 1 0
dense  = 3 4 0 0 0 0
n = 2

-> sparse[2] => 1
-> dense[1] = dense[n - 1]
-> sparse[dense[1]] = 1
