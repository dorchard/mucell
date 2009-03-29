import CSV
import Data.List
import Data.String
import System.Environment
import Text.ParserCombinators.Parsec

parseset fname = do
   res1 <- parseCSV ("data/"++fname++"-1-run.csv")
   res2 <- parseCSV ("data/"++fname++"-2-run.csv")
   res3 <- parseCSV ("data/"++fname++"-3-run.csv")
   res4 <- parseCSV ("data/"++fname++"-4-run.csv")
   res5 <- parseCSV ("data/"++fname++"-5-run.csv")
   return [res1, res2, res3, res4, res5]

foo = do
  res1 <- parseCSV ("data/a-1-run.csv")
  res <- return $ collect [res1]
  putStr $ show $ ((snd ((pairUp $ (head (dropTops (snd res))))!!30)) == 0.05)
  putStr $ show $ process $ pairUp $ (head (dropTops (snd res)))

main = do 
   args <- getArgs
   files <- mapM parseset args
   res <- return $ collect $ foldl (++) [] files
   case res of (False, errs) -> print $ show errs
 	       (True, xs) -> putStr $ show $ go $ dropTops $ xs

(+&+) :: (Bool, a) -> (Bool, [a]) -> (Bool, [a])
(b1, x) +&+ (b2, y) = (b1 && b2, x:y)

collect :: [Either a [[String]]] -> (Bool, [[[String]]])
collect [] = (True,[])
collect ((Left x):xs) = (False, []) +&+ (collect xs)
collect ((Right x):xs) = (True, x) +&+ (collect xs)

dropTops :: [[[String]]] -> [[[String]]]
dropTops = map (\x -> (drop 3 x)) 

go xs = let
    results = map (\x -> process $ pairUp x) xs
    maxResults = map (\x -> processMax $ pairUp x) xs
    mean = average results
    maxMean = average maxResults
    deviation = (foldl (+) 0.0 (map (\x -> abs (x - mean)) results)) / (toEnum (length xs))
    maxDeviation = (foldl (+) 0.0 (map (\x -> abs (x - maxMean)) maxResults)) / (toEnum (length xs))
    in
      (mean, deviation, maxMean, maxDeviation)

-- take a set of data and a sample index and return a pairing of the data for that sample
pairUp  :: [[String]] -> [(Double, Double)]
pairUp = pairUp' []

pairUp' ::  [(Double, Double)] -> [[String]] -> [(Double, Double)]
pairUp' ys [] = ys
pairUp' ys (x:xs) = 
  let
     a = read (x!!0) :: Double
     b = read (x!!1) :: Double
  in
     pairUp' ((a, b):ys) xs

-- average a list
average :: [Double] -> Double
average xs = (sum xs)/((toEnum (length xs))::Double)

-- Take a list of pairs of concentration and run
-- return max run length
processMax :: [(Double, Double)] -> Double
processMax = processMax' 0.0 0.0
-- returns set of run lengths
processMax' :: Double -> Double -> [(Double, Double)] -> Double
processMax' maxval length [] = max length maxval
processMax' maxval length ((_, run):xs) = 
              if run==0.05 then
                  processMax' (max maxval length) 0.0 xs
              else
                 processMax' maxval (length+0.05) xs

-- Take a list of pairs of concentration and run
-- return average set of run lengths
process :: [(Double, Double)] -> Double
process = average . process' 0.0 []
-- returns set of run lengths
process' :: Double -> [Double] -> [(Double, Double)] -> [Double]
process' l ys [] = l:ys
process' length ys ((_, run):xs) = 
              if run==0.05 then
                  if length==0.0 then
                      process' 0.0 ys xs
                  else
                      process' 0.0 (length:ys) xs
              else
                 process' (length+0.05) ys xs
