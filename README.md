# genetic-algorithm
Genetic algorithms exploration.

Applying Functional Progamming Prinicples, Parallel Progamming on Genetic Algorithm

Declartive Code call Starts at GNAlgorithm class at [GNAlgorithm.cs](SalesManProblem.Algorithms/Algorithm/GNA/GNAlgorithm.cs) file

* Configure the algorithm with the user choices:
  inside the Configure Method, I build the different algorithm parts via composition
```csharp
public class GNAlgorithm
{

    private readonly Func<Map,GNAResult> execute;

    public GNAlgorithm(GNAChoices choices,GNAOptions options)
    {
        execute = GNAConfigurations.Configure(choices, options);
    }
}
```

* Execute the algorithm with: Execute() method
```csharp
public GNAResult Execute(Map map)
{
    return execute(map);

}
```

* You can find the declarive configuration code at [GNAConfigurations.cs](SalesManProblem.Algorithms/Algorithm/GNA/GNAConfigurations.cs) file

* user can configure the algorithm from the choices at the advanced panel at the right of the UI (crossover, fitness calculation, selection, ... etc). user selection will reflect in the configure function by composing different pure function based on ui selection:
![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%202021-12-25%20143619.png)

* Screenshots of solving salesman problem with Genetic algorithm: 
![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2885).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2895).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2896).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2897).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2898).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2926).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2927).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2928).png)
