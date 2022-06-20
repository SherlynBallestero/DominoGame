using System.Collections.Generic;
using System;
 
namespace juego;
//definicion de que es una ficha
//que es una ficha, asumimos que es un objeto con caras y peso asignado.
public class Records
{
    //todos los elementos que contiene una ficha dada, es decir las caras 
 
    public int element1;
    public int element2;
    //aca se implementa un calculo de peso convencional de las fichas del domino 
    public int weight()
    {   
        return element1+element2;
    }
    //...constructor
    public Records(int element1,int element2)
    {
        this.element1=element1;
        this.element2=element2;
    }
}