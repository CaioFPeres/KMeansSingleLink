using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Ponto
{
  //varivel que guarda os valores do objeto na forma de ponto
  private string nome;
  private double x;
  private double y;

  //contrutor da classe
  public Ponto(double x, double y, string nome)
  {
    //sava os valores do ponto
    this.nome = nome;
    this.x = x;
    this.y = y;
  }

  //função que pega o valor d1/x do objeto/ponto
  public double getX()
  {
    return this.x;
  }

  //função que pega o valor d2/y do objeto/ponto
  public double getY()
  {
    return this.y;
  }

  //função que pega o nome do objeto/ponto
  public string getNome()
  {
    return this.nome;
  }
}
