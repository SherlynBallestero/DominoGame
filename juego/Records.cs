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
    public Records(int element1,int element2)
    {
        this.element1=element1;
        this.element2=element2;
    }
    // public override bool Equals(Records records)
    // {
    //     bool eq1=false;
    //     bool eq2=false;
    //     bool eq3=false;
    //     bool eq4=false;
    //     if(element1==records.element1){
    //         eq1=true;
    //         eq2=true;
    //         if(element2==records.element2)
    //         {
    //             eq3=true;
    //             eq4=true;
    //         }
    //     }
    //     else if (element1==records.element2){
    //         eq1=true;
    //         eq4=true;
    //         if(element2==records.element1)
    //         {
    //             eq3=true;
    //             eq2=true;
    //         }
    //     }
    //     return eq1 && eq2 && eq3 && eq4;
        
    // }
}