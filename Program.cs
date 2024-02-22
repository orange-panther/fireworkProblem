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

var mutex = new Semaphore(1, 1);
var s = new Semaphore[LITTLE_BOYS];
var state = new int[LITTLE_BOYS];
int leftLittleBoy;
int rightLittleBoy;

for (int i = 0; i < LITTLE_BOYS; i++)
{
    s[i] = new Semaphore(0, 1); // Initialize semaphore for each little boy
}

for (int i = 0; i < LITTLE_BOYS; i++)
{
    int littleBoy = i % LITTLE_BOYS;
    leftLittleBoy = (littleBoy + LITTLE_BOYS - 1) % LITTLE_BOYS;
    rightLittleBoy = (littleBoy + 1) % LITTLE_BOYS;
    LittleBoy(littleBoy);
}

Console.WriteLine("All little boys have lit their firework!");

void LittleBoy(int littleBoy)
{
    Starring(littleBoy); // Boy stares at the firework in the sky (thread sleeps)
    GrabLighter(littleBoy); // Boy tries to grab 2 lighters
    Light(littleBoy); // Boy lights the firework
    PutLighter(littleBoy); // Boy puts lighters down
}

void Starring(int littleBoy)
{
    // Little boy is looking at fireworks
    Console.WriteLine($"Little boy {littleBoy + 1} is staring at the firework...");
    Thread.Sleep(1000);
}

void Light(int littleBoy)
{
    // Boy lights the firework
    Console.WriteLine($"Little boy {littleBoy + 1} is lighting the firework...");
    Thread.Sleep(1000);
}

void GrabLighter(int littleBoy)
{
    mutex.WaitOne();
    state[littleBoy] = GRABBING;
    TryAcquireLighters(littleBoy);
    mutex.Release();
    s[littleBoy].WaitOne();
}

void PutLighter(int littleBoy)
{
    mutex.WaitOne();
    state[littleBoy] = STARRING;
    TryAcquireLighters(leftLittleBoy);
    TryAcquireLighters(rightLittleBoy);
    mutex.Release();
    Console.WriteLine($"Little boy {littleBoy + 1} has put down the lighter...");
    Thread.Sleep(1000);
}

void TryAcquireLighters(int littleBoy)
{
    if (state[littleBoy] == GRABBING && state[leftLittleBoy] != LIGHTING && state[rightLittleBoy] != LIGHTING)
    {
        state[littleBoy] = LIGHTING;
        s[littleBoy].Release(); // Signal the semaphore outside the if condition
    }
}