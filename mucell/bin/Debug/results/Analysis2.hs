import CSV
import Data.List
import Data.String
import System.Environment

main = do fname <- getArgs
          main' (head fname)

main' file = do
   res <- parseCSV file
   case res of Left err -> print err
 	       Right xs -> putStr $ show $ analyse xs

analyse :: [[String]] -> Double
analyse xs =
    let
        xs' = format $ drop 2 xs
    in
        (foldl (+) 0.0 (map (\n -> process $ pairUp xs' n) [0..4])) / 5.0

-- take a set of data and a sample index and return a pairing of the data for that sample
pairUp  :: [[String]] -> Int -> [(Double, Double)]
pairUp = pairUp' []

pairUp' ::  [(Double, Double)] -> [[String]] -> Int -> [(Double, Double)]
pairUp' ys [] _ = ys
pairUp' ys (x:xs) n = 
  let
     a = read (x!!n) :: Double
     b = read (x!!(n+5)) :: Double
  in
     pairUp' ((a, b):ys) xs n 

      
-- format data into lists each with 10 elements
format :: [[String]] -> [[String]]
format = format' []
format' :: [[String]] -> [[String]] -> [[String]]
format' ys [] = ys
format' ys (x:xs) = 
    let _:x' = (take 11 x)
        x'' = x' ++ (replicate (10-(length x')) [])
    in
       format' (x'':ys) xs

-- average a list
average :: [Double] -> Double
average xs = (sum xs)/((toEnum (length xs))::Double)

-- Take a list of pairs of concentration and run
-- return average set of run lengths
process :: [(Double, Double)] -> Double
process = average . process' 0.0 []
-- returns set of run lengths
process' :: Double -> [Double] -> [(Double, Double)] -> [Double]
process' l ys [] = l:ys
process' length ys ((_, run):xs) = 
              if run==0.0 then
                  if length==0.0 then
                      process' 0.0 ys xs
                  else
                      process' 0.0 (length:ys) xs
              else
                 process' (length+run) ys xs