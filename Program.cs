/* 
    Einzenberger, Vrhovac
    FIREWORK-PROBLEM SUMMARY:
    -----------------------
    5 little boys are sitting in a circle with a firework-rocket in front of them. The rockets 
    have 2 fuses each and you have to light them simultaneously. The Problem: There is only one 
    lighter between each of them.
*/
using System.Threading;

const int LITTLE_BOYS = 5;
const int STARRING = 0;
const int GRABBING = 1;
const int LIGHTING = 2;

var mutex = new Semaphore(1, 2);
var s = new Semaphore[LITTLE_BOYS];
var state = new int[LITTLE_BOYS];
int leftLittleBoy;
int rightLittleBoy;

for (int i = 0; true; i++)
{
    int littleBoy = i % (LITTLE_BOYS - 1);
    leftLittleBoy = (littleBoy + LITTLE_BOYS - 1) % LITTLE_BOYS;
    rightLittleBoy = (littleBoy + 1) % LITTLE_BOYS;
    LittleBoy(littleBoy);
}


void LittleBoy(int littleBoy)
{
    Starring(); //Boy stares at the firework in the sky (thread sleeps)
    GrabLighter(littleBoy); //Boy tries to grab 2 lighters
    Light(); //Boy lightes the firework
    PutLighter(littleBoy); //Boy puts lighters down
}

void Starring()
{
    //little boy is looking at fireworks
}

void Light()
{
    //boy lightes the firework
}


void GrabLighter(int littleBoy)
{
    // mutex.WaitOne();
    // state[littleBoy] = GRABBING;
    // TryAquireLighters(littleBoy);
    // mutex.Release();
    // s[littleBoy].WaitOne();

    lock (mutex)
    {
        state[littleBoy] = GRABBING;
        TryAquireLighters(littleBoy);
    }
    s[littleBoy].WaitOne();
}

void PutLighter(int littleBoy)
{
    mutex.WaitOne();
    state[littleBoy] = STARRING;
    TryAquireLighters(leftLittleBoy);
    TryAquireLighters(rightLittleBoy);
    mutex.Release();
}

void TryAquireLighters(int littleBoy)
{
    if (state[littleBoy] == GRABBING && state[leftLittleBoy] != LIGHTING && state[rightLittleBoy] != LIGHTING)
    {
        state[littleBoy] = LIGHTING;
        mutex.Release();
        s[littleBoy].Release(); // Signal the semaphore outside the if condition
    }
    else {
        mutex.Release(); // Consider releasing the mutex here if the condition is not met
    }
    

}