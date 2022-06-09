using System.Collections.Generic;
using System;
 
namespace juego;
//definicion de que es una ficha
//que es una ficha, asumimos que es un objeto con caras y peso asignado.
public class Records
{
    //todos los elementos que contiene una ficha dada, es decir las caras 
    public List<int> totalElements;
    //aca se implementa un calculo de peso convencional de las fichas del domino 
    public double weight()
    {
        int a=0;
        foreach (var item in totalElements)
        {
            a+=item;
        }
        return a;
    }
    //...constructor
    public Records(List<int> totalElements)
    {
        this.totalElements=totalElements;
    }
}