cp clay.h clay.c
gcc -shared -o clay.so -DCLAY_IMPLEMENTATION -fPIC clay.c
rm clay.h
