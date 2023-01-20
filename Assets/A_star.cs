using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_star : MonoBehaviour
{


    // tableau 2D pour représenter la carte
    int[,] map;
    // liste fermée des cases déjà visitées
    List<Vector2Int> closedList;
    // liste ouverte des cases à visiter
    List<Vector2Int> openList;
    List<Vector2Int> shortestPath = new List<Vector2Int>();
    // Start is called before the first frame update
   /*int[] dx = { -1, 1, 0, 0 };
   int[] dy = { 0, 0, -1, 1 };*/
    int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1 };
    int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1 };
    public static bool[,] Matrix = new bool[100, 100];
    Vector2Int start = new Vector2Int(1,1);
    Vector2Int end = new Vector2Int(11,11);

    public void CreateGrid()
    {

        // gridplane = new GameObject[rows, cols];
        //Matrix = new bool[100, 100];

        for (int x = 0; x < Matrix.GetLength(0); x++)
        {
            for (int y = 0; y < Matrix.GetLength(0); y++)
            {
                Matrix[x, y] = false;

            }
        }
        
    }



    void Start()
    {
        CreateGrid();
        Matrix[10, 10] = true;
        FindPath(start, end);


    }

    // Update is called once per frame
    void Update()
    {
        
    }




    //ladistance reelle par from start to node "x"
    public int getGcost(Vector2Int node, Vector2Int goal)
    {   
        int GCost = (int)Math.Sqrt(Math.Pow(node.x - goal.x, 2) + Math.Pow(node.y - goal.y, 2));
        return GCost;
    }




    public int ManhattanEstimateHcost(Vector2Int node, Vector2Int goal)
    {   //la distance de Manhattan séparant A et B  : (c'est la distance à parcourir jusquà l'arrivé)
        //la somme des déplacements horizontaux et verticaux nécessaires pour aller d'un point à un autre, sans tenir compte des obstacles.
        //successor.h = distance from goal to successor(This can be done using many ways, we will discuss three heuristics-  Manhattan, Diagonal and Euclidean  Heuristics)
        int hCost = Math.Abs(node.x - goal.x) + Math.Abs(node.y - goal.y);
        return hCost;
    }




    public int EuclideEstimateHcost(Vector2Int node, Vector2Int goal)
    {   //L'heuristique de Euclide est basée sur la distance Euclidienne, qui est une mesure de la distance entre deux points dans un espace à n dimensions
        int hCost = (int)Math.Sqrt(Math.Pow((node.x - goal.x),2)+ Math.Pow((node.x - goal.x), 2)) ;
        return hCost;
    }



    public int DiagonaleEstimateHcost(Vector2Int node, Vector2Int goal)
    {   //L'heuristique de Diagonale est basée sur la distance de Manhattan modifiée pour tenir compte des mouvements diagonaux
        int hCost = (int)(Math.Max(Math.Abs(node.x - goal.x), Math.Abs(node.y - goal.y))+(Math.Sqrt(2)-1)*Math.Min(Math.Abs(node.x-goal.x), Math.Abs(node.y-goal.y)));
        return hCost;
    }



    public int GetHeuristicCost_F(int g, int h)
    {
        //successor.f = successor.g + successor.h
        return (int)(g + h);
    }




    //fonction qui verifie si la cellule est bloqué
    public bool isBlocked(Vector2Int position)
    {
        return (Matrix[position.x, position.y] == true);
    }
    public void unBlockPosition(Vector2Int position)
    {
         Matrix[position.x, position.y] =false;
    }


    //*************************operation a* ********************
    List<Node> bubble_sort(List<Node> arr)
    {
        for (int i = 0; i < arr.Count - 1; i++)
        {
            for (int j = 0; j < arr.Count - i - 1; j++)
            {
                if (arr[j].getfCost() > arr[j + 1].getfCost())
                {
                   
                    
                    Node temp = arr[j+1];
                    arr[j + 1] = arr[j];
                    arr[j] = temp;

                }
            }
        }
        return arr;
    }

    



    // méthode pour récupérer les voisins d'un noeud
    private List<Node> GetNeighbors(Vector2Int position, Vector2Int end, Node parent)
    {
        List<Node> neighbors = new List<Node>();
        for(int i = 0; i< dx.Length; i++)
        {
            int nextx = (int)position.x + dx[i];
            int nexty = (int)position.y + dy[i];
            if (IsValidCoordinate(nextx, nexty))
            {
                Vector2Int currentpos = new Vector2Int(nextx, nexty);
               // successor.g = q.g + distance between successor and q
                int g = parent.getGcost() + getGcost(currentpos, end);

                int h = ManhattanEstimateHcost(currentpos, end);
                Node node = new Node(currentpos, g, h, parent);
                neighbors.Add(node);
               
            }

        }
        

        //List<Node> sorted = bubble_sort(neighbors);


        //return sorted;
        return neighbors;
    }





    public bool ContainsVector(List<Node> lists,Node value)
    {   //comparer les vecteurs
        for (int i = 0; i < lists.Count; i++)
        {
            if (lists[i].GetPosition() == value.GetPosition())
            {
                return true;
            }
        }
        return false;
    }




    // méthode pour vérifier si une coordonnée est valide (dans les limites de la matrice)
    private bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < Matrix.GetLength(0) && y >= 0 && y < Matrix.GetLength(0) && Matrix[x,y]==false;
    }





    private List<Vector2Int> RetracePath(Node node, Vector2Int startNode)
    {
        List<Node> path = new List<Node>();
        path.Add(node);

        // Retraçer les parents à partir du noeud d'arrivée jusqu'au noeud de départ
        Node currentNode = node;
        while (currentNode.GetPosition() != startNode)
        {
            // Ajouter le parent du noeud courant à la liste
            path.Add(currentNode.getParent());

            // Mettre à jour le noeud courant pour le parent
            currentNode = (currentNode.getParent());
        }


        // Inverser l'ordre de la liste pour qu'elle aille du noeud de départ au noeud d'arrivée
        path.Reverse();


        //add just vector element in this liste
        List<Vector2Int> pathVector2 = new List<Vector2Int>();
        for(int i = 0; i< path.Count; i++)
        {
            pathVector2.Add(path[i].GetPosition());
        }

        return pathVector2;
    
}



        //*** findpath
   public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
   {
        


        //Initialize the open list
        List<Node> openList = new List<Node>();
        //Initialize the closed list
        List<Node> closedList = new List<Node>();
        // ajouter le noeud de départ à la liste ouverte et mettre le f à 0
        int g = getGcost(start, end);
        int h = ManhattanEstimateHcost(start, end);
        
        Node startNode = new Node(start,g,h,null);
        openList.Add(startNode);

        Debug.Log("openList");

       

            //while the open list is not empty
            while (openList.Count > 0)
        {
            // trouver le noeud avec le coût f le plus faible dans la liste ouverte
            Node currentNode = null;
          

            foreach (Node node in openList)
            {
                if (currentNode == null || node.getfCost() < currentNode.getfCost())
                {
                    currentNode = node;
                }
            }



            // retirer le noeud courant de la liste ouverte et l'ajouter à la liste fermée (retirer le premier )
            openList.Remove(currentNode);
            closedList.Add(currentNode);


            //generate currentNode's 4 direction successors(child) and set their parents to currentNode
            List<Node> sorted = GetNeighbors(currentNode.GetPosition(), end, currentNode);


           
            // si le noeud courant est le noeud de fin, reconstruire le chemin le plus court
            foreach (var item in sorted)
            {  
                
                
                //if successor is the goal, stop search
                if (item.GetPosition() == end)
                {
                   
                    shortestPath = RetracePath(item, start);
                    string msg = "(";
                    foreach (var it in shortestPath)
                    {
                        msg += it.ToString() + ",";
                    }
                    msg += ")";
                    Debug.Log(msg);
                  
                    return shortestPath;
                }
                else
                {


                    //si le voisin estun obstacle ou  si le voisin est déjà dans la liste fermée
                    if ((Matrix[item.GetPosition().x, item.GetPosition().y] == true) || ContainsVector(closedList, item))
                    {   //cvd il retourne à la boucle suivant ,sans compiler le reste
                        continue;

                    }  
                        int index = openList.IndexOf(item);

                        
                    //if (ContainsVector(openList, item) == false)
                        if (openList.Contains(item) == false)
                        {
                            openList.Add(item);
                        
                        }
                        else if (item.getGcost() < openList[index].getGcost())
                        {

                            openList[index].setGcost(item.getGcost());
                            openList[index].setParent(item.getParent());
                            //openList[index] = item;
                           

                            //openList[neighbor].g = g;
                            //openList[neighbor].parent = currentNode;
                        }
                    

                }
            }
           
        }

        // pas de chemin trouvé
        //return new List<Vector2Int>() ;
        return new List<Vector2Int>();
    }

}
