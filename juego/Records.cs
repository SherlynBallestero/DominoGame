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
    
    //...constructor
    public Records(List<int> elements)
    {
        this.element1=elements[0];
        this.element2=elements[1];
    }
  
}