from collections import namedtuple


Point = namedtuple("Point", "x y")

setattr(Point, "x", 4)

setattr(Point, "x", 6)
p = Point(1, 2)

print(Point.x)
print(type(p))
print(p)
