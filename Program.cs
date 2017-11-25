using System;
using System.Collections.Generic;

namespace vsCode
{
    class Program
        {
            static void Main(string[] args)
            {
            string[] blockedDom = new string[] { "list.stolen.credit.card.us", "piratebay.co.uk", "visitwar.de", "microvirus.md" }; //normally these would be read from .txt or similar
                string[] inputDom = new string[] { "unlock.microvirus.md", "visitwar.com", "visitwar.de", "fruonline.co.uk", "australia.open.com", "credit.card.us" }; 
                int[] ourAllowedInd = Solution.solution(inputDom, blockedDom);
                Console.WriteLine(ourAllowedInd);
            }


        public class Solution 
        {
            public static int[] solution(string[] inputDom, string[] blockedDom)
            {
                int[] allowedIndexes = new int[inputDom.Length];
                string[] blockedDomNoDots = new string[blockedDom.Length];
                string[] inputDomNoDots = new string[inputDom.Length];
                bool[,,,,,] mappingWebsites = new bool [26, 26, 26, 26, 26, 26];
                Dictionary<int, List<string>> blockedSitesDict = new Dictionary<int, List<string>>();//the int is the hashcode of last characters on the domain. See method GetHashCodes
                List<int> notBlockedIndexes = new List<int>();

                for (int i = 0; i < blockedDom.Length; i++) //we will assign block domains to mapping / bucket based on the last 6 non-dot characters in the domain
                {  //we will then assign a TRUE value to for any bucket with specific 6 characters where a corresponding blocked domain exists.
                    blockedDomNoDots[i]= blockedDom[i].Replace(".",""); //removing dots from domain
                    int charLimit = 6;
                    if (blockedDomNoDots[i].Length<6)
                    {
                        charLimit = blockedDomNoDots[i].Length;
                    }
                
                    int ourLength1 = blockedDomNoDots[i].Length;
                    int[] indexChars = new int[charLimit];
                    for (int i2 = 0; i2 < charLimit ; i2++) //assigning domain to the appropriate mapping / bucket based on the last 6 characters of the domain
                    {
                        
                        char ourChar = blockedDomNoDots[i][ourLength1 - (1+i2)];
                        indexChars[i2] = char.ToUpper(ourChar) - 64;
                    }     
                    mappingWebsites[indexChars[0],indexChars[1],indexChars[2],indexChars[3],indexChars[4],indexChars[5]] = true;//to be fixed with charLimit for domains with less than 6 characters
                    int hashC = GetHashCode(indexChars);
                    if (!blockedSitesDict.ContainsKey(hashC)) //if no other domains in this mapping
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(blockedDom[i]);
                        
                        blockedSitesDict.Add(hashC,tempList);
                    }
                    else if (blockedSitesDict.ContainsKey(hashC)) //if there are already other domains in this mapping
                    {
                        blockedSitesDict[hashC].Add(blockedDom[i]);
                    }
                    //a new class needs to be created. TBC
                }
                // int charLimit2 = 6;
                for (int i = 0; i < inputDom.Length; i++) //we will assign block domains to mapping / bucket based on the last 6 non-dot characters in the domain
                {  //we will then assign a TRUE value to for any bucket with specific 6 characters where a corresponding blocked domain exists.
                    inputDomNoDots[i]= inputDom[i].Replace(".",""); //removing dots from domain
                    int charLimit2 = 6;
                    // if (inputDomNoDots[i].Length<6)
                    // {
                    //     charLimit2 = inputDomNoDots[i].Length;
                    // }
                
                    int ourLength = inputDomNoDots[i].Length;
                    int[] indexChars2 = new int[charLimit2];
                    for (int i2 = 0; i2 < charLimit2 ; i2++) //assigning domain to the appropriate mapping / bucket based on the last 6 characters of the domain
                    {
                        
                        char ourChar = inputDomNoDots[i][ourLength - (1+i2)];
                        indexChars2[i2] = char.ToUpper(ourChar) - 64;
                    }     
                if (mappingWebsites[indexChars2[0],indexChars2[1],indexChars2[2],indexChars2[3],indexChars2[4],indexChars2[5]] ==  true)
                {
                    
                        int hashC2 = GetHashCode(indexChars2);
                        foreach (string blockedDomain in blockedSitesDict[hashC2]) //this should be a while. to be changed
                        {
                            if (inputDom[i].Contains(blockedDomain)) // NEED TO CHECK IF WHOLE STRING IS INCLUDED IN BLOCKED ONES
                            {  
                                Console.WriteLine(string.Format("found one! "+ inputDom[i] +" is blocked"));
                            }
                            else
                            {
                                Console.WriteLine(inputDom[i] + " is NOT blocked");
                                notBlockedIndexes.Add(i);
                            }
                        }
                        
                }else if (mappingWebsites[indexChars2[0],indexChars2[1],indexChars2[2],indexChars2[3],indexChars2[4],indexChars2[5]] ==  false)
                {
                    Console.WriteLine(inputDom[i] + " is NOT blocked");
                        notBlockedIndexes.Add(i);
                }
                }
                notBlockedIndexes.ForEach(i => Console.Write("{0}\t", i));
                int[] allowedIndexesArr = notBlockedIndexes.ToArray();

                return allowedIndexesArr;
            }

            static int GetHashCode(int[] values) //this is used to get a hashcode to refer to the mappings easily.
                    {
                    int result = 0;
                    int shift = 0;
                        for (int i = 0; i < values.Length; i++)
                        {
                            shift = (shift + 11) % 21;
                            result ^= (values[i]+1024) << shift;
                        }
                    return result;
                    }
        }
    }
}