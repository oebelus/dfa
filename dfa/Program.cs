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
            { "A", new Dictionary<string, List<string>> { { "0", ["A", "B"] }, {"1", ["C"] } } },
            { "B", new Dictionary<string, List<string>> { { "0", ["A"] }, {"1", ["B"] } } },
            { "C", new Dictionary<string, List<string>> { { "0", [""] }, {"1", ["A", "B"] } } }
        };
        
        //string firstState = "A";
        string finalState = "C";
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
        foreach (var el in finals) Console.WriteLine(el);
        DFA(dfa, "11", "A",  finals);
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

    // Setting the final states
    static List<string> ToFinalStates(Dictionary <string, Dictionary<string, string>> dfa, string finalState) {
        List <string> finalStates = [];
        foreach (var (key, _) in dfa) {
            if (key.Contains(finalState)) finalStates.Add(key);
        }
        return finalStates;
    }

    // NFA to DFA conversion
    static Dictionary <string, Dictionary<string, string>> ToDFA(Dictionary <string, Dictionary<string, List<string>>> adjacency, int statesCount, List <string> keys) {
        Dictionary <string, Dictionary<string, string>> dfa = [];
        Dictionary <string, Dictionary<string, string>> later = [];
        List <string> newStates = [];
        List<string> keysToRemove = [];
        
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        foreach (var (a, b) in adjacency) {
            foreach (var (_, val) in b) {
                string newState = "";
                if (val.Count > 1)
                    foreach (var el in val) newState += el;
                if (!string.IsNullOrEmpty(newState) && !newStates.Contains(SortString(newState).Trim())) newStates.Add(SortString(newState.Trim()));
            }
        }

        Dictionary<string, string> toAdd = [];
        foreach (var (key, val) in adjacency.First().Value)
            toAdd.Add(key, string.Join("", val));

        dfa.Add(adjacency.First().Key, toAdd);
        foreach (var (_, val) in toAdd) {
            if (adjacency.TryGetValue(val, out Dictionary<string, List<string>>? value)) {
                for (int k = 0; k < keys.Count; k++) {
                    if (adjacency.ContainsKey(val)) {
                        var input = keys[k];
                        if (!dfa.ContainsKey(val)) {
                            dfa.Add(val, new Dictionary<string, string>{{input, string.Join("", value[input])}});
                            newStates.Add(SortString(string.Join("", value[input])));
                        } else {
                            dfa[val].TryAdd(input, string.Join("", value[input]));
                            newStates.Add(SortString(string.Join("", value[input])));
                        }
                    }
                }
            } else {
                // State 1 U State 2 
                List<HashSet<string>> sets = [];
                for (int a = 0; a < keys.Count; a++) {
                    sets.Add([]);
                }
                List<string> newEdges = [];
                for (int b = 0; b < keys.Count; b++) {
                    newEdges.Add("");
                }

                foreach (char character in val) {
                    if (adjacency.ContainsKey(character.ToString())) {
                        string state = character.ToString();
                        for (int k = 0; k < keys.Count; k++) {
                            string key = keys[k];
                            string word = string.Join("", adjacency[state][key]);
                            for (int w = 0; w < word.Length; w++) {
                                sets[k].Add(word[w].ToString());
                            }
                            if (!dfa.TryGetValue(val, out Dictionary<string, string>? valeur)) {
                                dfa.Add(val, new Dictionary<string, string>{{key, string.Join("", sets[k])}});
                                newStates.Add(SortString(string.Join("", sets[k])));
                            }
                            else {
                                valeur[key] = SortString(string.Join("", sets[k]));
                                newStates.Add(SortString(string.Join("", sets[k])));
                            }
                        }
                    }
                }
            } 
        }

        for (int i = 0; i < newStates.Count; i++) {
            if (!dfa.ContainsKey(newStates[i])) {
                List<HashSet<string>> sets = [];
                for (int a = 0; a < keys.Count; a++) {
                    sets.Add([]);
                }

                foreach (char character in newStates[i]) {
                    string state = character.ToString();

                for (int k = 0; k < keys.Count; k++) {
                        string key = keys[k];
                        string word = string.Join("", adjacency[state][key]);

                        for (int w = 0; w < word.Length; w++) {
                            sets[k].Add(word[w].ToString());
                        }
                        if (!dfa.TryGetValue(newStates[i], out Dictionary<string, string>? valeur)) {
                            dfa.Add(newStates[i], new Dictionary<string, string>{{key, string.Join("", sets[k])}});
                        }
                        else {
                            valeur[key] = SortString(string.Join("", sets[k]));
                        }

                        // Add created merged states to the newStates List (A, B -> AB)
                        if (SortString(string.Join("", sets[k]).Trim()).Length > 1)
                            newStates.Add(SortString(string.Join("", sets[k])));
                    }
                }
            }
        

        // Filling the empty values with unused characters
        foreach (var outer in dfa)
        {
            int countEmpty = 0;
            foreach (var inner in outer.Value)
            {
                if (string.IsNullOrEmpty(inner.Value)) countEmpty++;
                    if (countEmpty == keys.Count) keysToRemove.Add(outer.Key);
                    else if (countEmpty > 0)
                    {
                        for (int k = 0; k < keys.Count; k++) {
                            string key = keys[k];
                            if (string.IsNullOrEmpty(outer.Value[key])) {
                                for (int j = 0; j < alphabet.Length; j++) {
                                    if (!adjacency.ContainsKey(alphabet[j].ToString())) {
                                        outer.Value[key] = alphabet[j].ToString();
                                        Dictionary<string, string> newKeys = [];
                                        for (int l = 0; l < keys.Count; l++) {
                                            newKeys.Add(keys[l], alphabet[j].ToString());
                                        }
                                        later.Add(alphabet[j].ToString(), newKeys);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }  
            } 
        }

        foreach (string key in keysToRemove)
        {
            if (!newStates.Contains(key))
                dfa.Remove(key);
        }

        foreach (var (key, el) in later) dfa[key] = el;
        
        Dictionary<string, Dictionary<string, string>> dfatest = [];

        foreach (var outerPair in adjacency)
        {
            Dictionary<string, string> innerDict = [];
            foreach (var innerPair in outerPair.Value)
            {
                innerDict.Add(SortString(innerPair.Key), SortString(string.Join("", innerPair.Value)));
            }
            dfatest.Add(outerPair.Key, innerDict);
        }

        // Print out the original adjacency list
        foreach (var (a, b) in dfatest) {
            Console.Write(a);
            foreach (var (_, d) in b) Console.Write(d.PadLeft(8));
                Console.WriteLine();
        }

        Console.WriteLine("----------------------------");

        return dfa;
    }

    // Print a DFA
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

    // Function to sort strings in ascending order
    static string SortString(string input)
    {
        char[] characters = [.. input];
        Array.Sort(characters);
        return new string(characters);
    }
}