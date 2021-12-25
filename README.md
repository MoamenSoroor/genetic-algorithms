# genetic-algorithm
Genetic algorithms exploration.

Applying Functional Progamming Prinicples, Parallel Progamming on Genetic Algorithm

Declartive Code call Starts at GNAlgorithm class at [call of declaritive part](SalesManProblem.Algorithms/Algorithm/GNA/GNAlgorithm.cs)

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



![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2885).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2895).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2896).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2897).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2898).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2926).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2927).png)

![alt text](https://github.com/MoamenSoroor/genetic-algorithms/blob/master/Screenshots/Screenshot%20(2928).png)
