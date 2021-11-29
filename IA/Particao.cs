using System;
using System.Collections.Generic;

public class Particao
{
  //variavel que guarda o numero de cluster atual
  protected int numCluster;
  
  //get para pegar o numero de clusters
  public int getNumCluster(){
    return numCluster;
  }

  //set que atualiza o numero de cluster
  public void setNumCluster(int numClust){
    this.numCluster = numClust;
  }

  //função que calcula a distancia euclidiana de dois pontos
  public double DistanciaEuclidiana(Ponto ponto1, Ponto ponto2)
  {
    //variaveis que guardam os x e y de dois pontos
    double x1, y1, x2, y2;

    //pega os valores x e y de cada ponto
    x1 = ponto1.getX();
    x2 = ponto2.getX();

    y1 = ponto1.getY();
    y2 = ponto2.getY();

    //retorna o resultado do calculo da distancia euclidiana
    return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
  }

  //retorna centróide de cada cluster
  public Ponto media(List<Ponto> cluster)
  {
    //variaveis usadas para encontrar os centroides dos clusters
    double x = 0, y = 0;
    foreach (Ponto pontoAtual in cluster)
    {
      //faz a somatoria de todos os valores x e y dos pontos no cluster
      x = x + pontoAtual.getX();
      y = y + pontoAtual.getY();
    }
    //acha a media, dividindo o resultado da somatoria pelo numero de pontos
    x = x/cluster.Count;
    y = y/cluster.Count;
    //cria um ponto que representa o centroide
    Ponto media = new Ponto(x,y, "Temp");
    
    //retorna o centroide
    return media;
  }
}