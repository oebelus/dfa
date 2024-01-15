// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using System;
using System.Collections.Generic;

public class FA
{
    public static void Main(string[] args)
    {
        // Uncomment this, and comment the uncommented (XD) code for the interactive testing of your DFA
            /*Console.WriteLine("Enter the number of states");
            string? statesCount = Console.ReadLine();
            List <string> states = new();
            Console.WriteLine("Enter the states");
            if (statesCount == null) throw new ArgumentNullException(nameof(statesCount));
            for (int i = 0; i < int.Parse(statesCount); i++) {
                string? state = Console.ReadLine();
                if (state == null) throw new ArgumentNullException(nameof(state));
                states.Add(state);
            }
            Console.WriteLine("Enter the initial state");
            string? firstState = Console.ReadLine();
            Console.WriteLine("Enter the final state");
            string? finalState = Console.ReadLine();
            if (finalState == null) throw new ArgumentNullException(nameof(finalState));
            List<string> finalStates = [finalState];
            Console.WriteLine("Enter the number of keys");
            string? keysCount = Console.ReadLine();
            List <string> keys = new();
            Console.WriteLine("Enter the keys");
            if (keysCount == null) throw new ArgumentNullException(nameof(keysCount));
            for (int i = 0; i < int.Parse(keysCount); i++) {
                string? key = Console.ReadLine();
                if (key == null) throw new ArgumentNullException(nameof(key));
                keys.Add(key);
            }
        
            Dictionary <string, Dictionary<string, string>> adjacency = new();
            foreach (string state in states) {
                if (!adjacency.ContainsKey(state)) adjacency.Add(state, new Dictionary<string, string>());
            }
            
            for (int j = 0; j < int.Parse(statesCount); j++) {
                foreach (string key in keys) {
                    Console.WriteLine($"{states[j]} {key}");
                    string? input = Console.ReadLine();
                    
                    foreach (var (state, dic) in adjacency) {
                        if (state == states[j].ToString()) {
                            if (input == null) throw new ArgumentNullException(nameof(input));
                            adjacency[state].Add(key.ToString(), input);
                        }
                    }
                }
            }
            
            foreach (var (a, b) in adjacency) {
                Console.Write(a);
                foreach (var c in b) Console.Write($" {c} ");
                    Console.WriteLine();
                }
            int counter = 0;
            while (counter < 5) {
                Console.WriteLine("Enter test string");
                string? test = Console.ReadLine();
                if (test == null) throw new ArgumentNullException(nameof(test));
                if (firstState == null) throw new ArgumentNullException(nameof(firstState));
                if (finalState == null) throw new ArgumentNullException(nameof(finalState));
                DFA(adjacency, test, firstState, finalStates);
                counter++;
            }
        }*/
        List <string> keys = ["0", "1"];
        Dictionary<string, Dictionary<string, List<string>>> states = new()
        {
            { "A", new Dictionary<string, List<string>> { { "0", ["B", "C"] }, {"1", ["A"] } } },
            { "B", new Dictionary<string, List<string>> { { "0", [""] }, {"1", ["B"] } } },
            { "C", new Dictionary<string, List<string>> { { "0", ["C"] }, {"1", ["C"] } } },
        };
        
        //string firstState = "A";
        string finalState = "B";
        int statesLength = 0; 
        
        foreach (var (a, b) in states) {
            Console.Write($"{a}   ");
            statesLength++;
            foreach (var (key, val) in b) {
                Console.Write($"{key}: ");
                Console.Write("[ ");
                for (int i = 0; i < val.Count; i++) Console.Write($"{val[i]} ");
                Console.Write("] ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("---------------------------");
        var dfa = ToDFA(states, 2, keys);
        Print(dfa, keys);
        Console.WriteLine("---------------------------");
        List<string> finals = ToFinalStates(dfa, finalState);
        DFA(dfa, "011000", "A",  finals);
    }
    static void DFA(Dictionary <string, Dictionary<string, string>> adjacency, string input, string firstState, List<string> finalState) {
        string currentState = firstState;
        foreach (char c in input) {
            foreach (var (edge, key) in adjacency[currentState]) {
                if (edge == c.ToString()) {
                    currentState = key;
                    break;
                }
            }
        }
        if (finalState.Contains(currentState)) Console.WriteLine("Yes");
        else Console.WriteLine("No");
    }
    static List<string> ToFinalStates(Dictionary <string, Dictionary<string, string>> dfa, string finalState) {
        List <string> finalStates = [];
        foreach (var (key, _) in dfa) {
            if (key.Contains(finalState)) finalStates.Add(key);
        }
        return finalStates;
    }
    static Dictionary <string, Dictionary<string, string>> ToDFA(Dictionary <string, Dictionary<string, List<string>>> adjacency, int statesCount, List <string> keys) {
        List <string> newStates = [];
        
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        foreach (var outer in adjacency)
        {
            foreach (var inner in outer.Value)
            {
                if (inner.Value.Count == 1 && string.IsNullOrEmpty(inner.Value[0]))
                {
                    for (int i = 0; i < alphabet.Length; i++) {
                        if (!adjacency.ContainsKey(alphabet[i].ToString())) {
                            inner.Value[0] = alphabet[i].ToString();
                            newStates.Add(alphabet[i].ToString());
                            break;
                        }
                    }
                }
            }
        }

        foreach (var (a, b) in adjacency) {
            foreach (var (key, val) in b) {
                string newState = "";
                if (val.Count > 1)
                    foreach (var el in val) newState += el;
                newStates.Add(SortString(newState));
            }
        }

        for (int i = 0; i < newStates.Count; i++) {
            var rev = newStates[i].ToCharArray().Reverse();
            if (!adjacency.ContainsKey(newStates[i]) && !adjacency.ContainsKey(string.Join("", rev)) && !string.IsNullOrEmpty(newStates[i])) {
                HashSet<string> set0 = [];
                HashSet<string> set1 = [];
                HashSet<string> notThere = [];
                string zeros = "";
                string ones = "";
                int length = newStates[i].Length;
                for (int j = 0; j < length; j++) {
                    if (adjacency.ContainsKey(newStates[i][j].ToString())) {
                        var temp0 = adjacency[newStates[i][j].ToString()][keys[0]];
                        if (!string.IsNullOrEmpty(temp0[0].Trim())) { 
                            for (int k = 0; k < temp0.Count; k++) {
                                set0.Add(SortString(temp0[k]));
                            }
                        }
                        var temp1 = adjacency[newStates[i][j].ToString()][keys[1]];
                        if (!string.IsNullOrEmpty(temp1[0].Trim())) {
                            for (int k = 0; k < temp1.Count; k++) {
                                set1.Add(SortString(temp1[k]));
                            }
                        }
                    } else {
                        set0.Add(newStates[i][j].ToString());
                        set1.Add(newStates[i][j].ToString());
                    }
                }
                foreach (string el in set0) zeros += el;
                foreach (string el in set1) ones += el;
                newStates.Add(SortString(zeros));
                newStates.Add(SortString(ones));
                adjacency.Add(newStates[i], new Dictionary<string, List<string>> {{keys[0], new List<string> {zeros}},{keys[1], new List<string> {ones}}});
                statesCount++;
            }
        }
        
        foreach (var (a, b) in adjacency) {
            foreach (var key in b.Keys.ToList()) {
                List<string> val = b[key];
                string newState = "";
                if (val.Count > 1) {
                    newState = string.Join("", val);
                    b[key] = [SortString(newState)];
                }
            }
        }
        List<string> keysToRemove = new List<string>();
        
        foreach (string key in keysToRemove)
        {
            adjacency.Remove(key);
        }
        Dictionary<string, Dictionary<string, string>> dfa = [];

        foreach (var outerPair in adjacency)
        {
            Dictionary<string, string> innerDict = [];
            foreach (var innerPair in outerPair.Value)
            {
                innerDict.Add(SortString(innerPair.Key), SortString(string.Join("", innerPair.Value)));
            }
            dfa.Add(outerPair.Key, innerDict);
        }

        return dfa;
    }
    static void Print(Dictionary <string, Dictionary<string, string>> adjacency, List<string> keys) {
        foreach (string el in keys) {
            Console.Write(el.PadLeft(8));
        }
        Console.WriteLine();
        foreach (var (a, b) in adjacency) {
            Console.Write(a);
            foreach (var (c, d) in b) Console.Write(d.PadLeft(8));
                Console.WriteLine();
        }
    }
    static string SortString(string input)
    {
        char[] characters = [.. input];
        Array.Sort(characters);
        return new string(characters);
    }
}