using System.Collections.Generic;
using System.Linq;

public class ClusterSL
{
  //variavel que guarda uma lista com todos os pontos nesse cluster
  private List<Ponto> pontos;
  //lista que guarda a distancia relevante ao single link do cluster atual até todos os outros cluster  
  private List<double> dist;

  //contrutor que cria o cluster
  public ClusterSL(List<Ponto> pontos){
    this.pontos = pontos;
  }

  //função que retorna a lista de pontos
  public List<Ponto> getPontos(){
    return pontos;
  }

  //função que retorna a distancia do node atual até um node de id especifico
  public double getDist(int idDist){
    return dist.ElementAt(idDist);
  }

  //função que retorna um ponto de id especifico
  public Ponto getPonto(int idPonto){
    return pontos.ElementAt(idPonto);
  }

  //função que substitui a lista de distancias atual por outra lista
  public void setDist(List<double> dist){
    this.dist = dist;
  }

  //função que adiciona um ponto ao cluster
  public void addPonto(Ponto ponto){
    this.pontos.Add(ponto);
  }

  //função que remove a distancia do cluster atual até um cluster de id especifico da lista de distancias
  public void removeDist(int idDist){
    this.dist.RemoveAt(idDist);
  }
  
  //função que muda a distancia do cluster atual até um cluster de id especifico
  public void setDistId(int idDist, double dista){
    this.dist[idDist] = dista;
  }
}