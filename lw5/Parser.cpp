// Parser.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <string>
#include <cctype>
#include <sstream>
#include <set>
#include <set>

using namespace std;

set <string> TypeSet = {"int", "float", "bool", "string"};
set <char> IdNameSet = {' ', ',', 'i', 'd'};
const string id = "id";
const string num = "num";
const string VarTocken = "VAR";
const string ReadTocken = "READ";
const string WriteTocken = "WRITE";
const string AssignTocken = "id:=";
const string ProgTocken = "PROG id";
const string BeginTocken = "BEGIN";
const string EndTocken = "END";
void IdListRec(string& St, int& Count)
{
    if (St[0] == 'i')
    {
        Count -= 1;
    }
    if (St[0] == ',')
    {
        Count = Count * 2;
    }
    St = St.substr(1);
    if (St != "")
    {
        IdListRec(St, Count);
    }
}
bool IdList(string& St)
{
    string helpSt;
    int i, j = 0, Count = 2;
    for (i = 0; i < St.length(); i++)
    {
        if (St[i] == 'i')
        {
            helpSt += St[i];
            j++;
        }
        if (St[i] == 'd')
        {
            j--;
        }
        if (St[i] == ',')
        {
            helpSt += St[i];
        }
        if (!IdNameSet.contains(St[i]))
        {
            return false;
        }
    }
    IdListRec(helpSt, Count);
    if (Count == 1)
    {
        return true;
    }
    else
    {
        return false;
    }
}
bool VarSt(string& St)
{
    string VarCheck, idList, Type;
    for (int i = 0; i < 3; i++)
    {
        VarCheck += St[i];
    }
    if (VarCheck != VarTocken)
    {
        return false;
    }
    idList = St.substr(3, St.find_first_of(':') - 3);
    if (!IdList(idList))
    {
        return false;
    }
    Type = St.substr(St.find_first_of(':')+1, St.find_first_of(';') - St.find_first_of(':') - 1);
    if (!TypeSet.contains(Type))
    {
        return false;
    }
    St = St.substr(St.find_first_of(';') + 1);
    return true;
}
bool Read(string& St)
{
    string Vir;
    Vir = St.substr(St.find_first_of('(') + 1, St.find_last_of(')') - St.find_first_of('(') - 1);
    if (!IdList(Vir))
    {
        return false;
    }
    return true;
}
bool Write(string& St)
{
    string Vir;
    Vir = St.substr(St.find_first_of('(') + 1, St.find_last_of(')') - St.find_first_of('(') - 1);
    if (!IdList(Vir))
    {
        return false;
    }
    return true;
}
bool Exp(string& St);
bool T(string& St);
bool F(string& St)
{
    if (St[0] == '-')
    {
        St = St.substr(1);
        return F(St);
    }
    if (St[0] == '(')
    {
        St = St.substr(1);
        if (Exp(St))
        {
            return (St[St.length()-1] == ')');
        }
    }
    if (St[0] == 'i')
    {
        St = St.substr(1);
        return true;
    }
    if (St[0] == 'n')
    {
        St = St.substr(1);
        return true;
    }
    return true;
}
bool C(string& St)
{
    if (St[0] == '+')
    {
        St = St.substr(1);
        if (T(St))
        {
            return C(St);
        }
    }
    return true;
}
bool D(string& St)
{
    if (St[0] == '*')
    {
        St = St.substr(1);
        if (F(St))
        {
            return D(St);
        }
        return false;
    }
    return true;
}
bool T(string& St)
{
    if (!F(St))
    {
        return false;
    }
    return D(St);
}
bool Exp(string& St)
{
    if (!T(St))
    {
        return false;
    }
    return C(St);
}
bool Assign(string& St)
{
    string Vir = "", helpSt = "";
    helpSt = St.substr(4);
    for (int i = 0; i < helpSt.length(); i++)
    {
        if (helpSt[i] != 'd')
        {
            if (helpSt[i] != 'u')
            {
                if (helpSt[i] != 'm')
                {
                    if (helpSt[i] != ' ')
                    {
                        Vir += helpSt[i];
                    }
                }
            }
        }
    }
    if (!Exp(Vir))
    {
        return false;
    }
    return true;
}
bool ST(string& St)
{
    string Operator = St.substr(0, 4);
    if (Operator == ReadTocken)
    {
        Read(St);
    }
    else
    {
        if (Operator == AssignTocken)
        {
            Assign(St);
        }
        else
        {
            Operator += St[4];
            if (Operator == WriteTocken)
            {
                Write(St);
            }
            else
            {
                return false;
            }
        }
    }
    return true;
}

bool ListSt(string& St)
{
    string First, Operator;
    if (St == "")
    {
        return true;
    }
    int FirstDot = St.find_first_of(';') + 1;
    First = St.substr(0, FirstDot);
    if (!ST(First))
    {
        return false;
    }
    St = St.substr(FirstDot, St.length() - FirstDot);
    ListSt(St);
    return true;
}

bool Prog(string& St)
{
    string helpSt = "", List = "";
    int i = 0;
    for (i = 0; i <= 6; i++)
    {
        helpSt += St[i];
    }
    if (helpSt != ProgTocken)
    {
        return false;
    }
    St = St.substr(7);
    helpSt = "";
    for (i = 0; i < St.length(); i++)
    {
        if (St[i] != ' ')
        {
            helpSt += St[i];
        }
    }
    St = helpSt;
    helpSt = "";
    if (!VarSt(St))
    {
        return false;
    }
    for (i = 0; i < 5; i++)
    {
        helpSt += St[i];
    }
    if (helpSt != BeginTocken)
    {
        return false;
    }
    St = St.substr(5);
    List = St.substr(0, St.find_last_of(';') + 1);
    St = St.substr(St.find_last_of(';') + 1);
    if (!ListSt(List))
    {
        return false;
    }
    if (St != EndTocken)
    {
        return false;
    }
    return true;
}
int main()
{
    string St = "PROG id  VAR id, id, id : int; BEGIN READ(id,id); id:=id+num*-(-num+id*id*-num*( id+-num)); WRITE(id,id,id); END";
    if (Prog(St))
    {
        cout << "Dance or die!!!";
    }
    

}