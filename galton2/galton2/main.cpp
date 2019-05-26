#include <iostream>
#include <ctime>
#include <cstdlib>
using namespace std;


class Galton_erfen{
private:
    int ballnum;
    int erfentimes;
    int *result;
public:
    Galton_erfen(int num=1000,int times=100)
    {
        ballnum=num;
        erfentimes=times;
        result=new int[times+1];
        for(int i=0;i<times+1;i++) result[i]=0;
    }
    void simu();
    void display();
};

void Galton_erfen::simu()
{
    srand(time(NULL));
    for(int i=0;i<ballnum;i++)
    {
        int state=0;
        for(int j=0;j<erfentimes;j++)
        {
            int tmp=rand()%2;
            if(tmp==1) state += 1;
        }
        result[state] += 1;
    }
}

void Galton_erfen::display()
{
    for(int i=0;i<erfentimes+1;i++)
        cout<<i<<"  "<<result[i]<<endl;
}

int main()
{

    Galton_erfen g;

    g.simu();
    g.display();
    return 0;
}
