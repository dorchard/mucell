import CSV
import Data.List
import Data.String
import System.Environment

main = do fname <- getArgs
          main' (head fname)

main' file = do
   res <- parseCSV file
   case res of Left err -> print err
 	       Right xs -> putStr $ "Concentration Time\n" ++ (tocsv $ analyse xs)

tocsv :: [(Double, Double)] -> String
tocsv [] = ""
tocsv ((a, b):xs) = (show a)++" "++(show b)++"\n"++(tocsv xs)


grouping :: [(Double, Double)] -> [(Double, Double)]
grouping = grouping' 0.0 0.05 [] []
grouping' :: Double -> Double -> [(Double, Double)] -> [(Double, Double)] -> [(Double, Double)] -> [(Double, Double)]
grouping' _ _ [] ys [] = reverse ys
grouping' _ _ g ys [] = reverse ((pairAverage g):ys)
grouping' previous threshold group ys ((x1, y1):xs) = 
  if x1<=(previous+threshold) then
      grouping' previous threshold ((x1, y1):group) ys xs
  else
      grouping' x1 threshold [] ((pairAverage group):ys) xs

analyse :: [[String]] -> [(Double, Double)]
analyse xs =
    let
        xs' = format $ drop 2 xs
    in
       compress $ sortBy pairSort $ foldl (++) [] (map (\n -> process $ pairUp xs' n) [0..4])

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

-- pair average
pairAverage :: [(Double, Double)] -> (Double, Double)
pairAverage xs = let len = ((toEnum (length xs))::Double)
                 in ((foldl (\y (a,b) -> a+y) 0.0 xs)/len, (foldl (\y (a,b) -> b+y) 0.0 xs)/len)

-- average run lengths on a pairing of concentraion, runlength
runLengthAverage :: [(Double, Double)] -> (Double, Double)
runLengthAverage xs = let (c, _):_ = xs
                      in (c,  average (map (\(x,y) -> y) xs))

-- sort based on 1st element of tuple
pairSort :: (Double, Double) -> (Double, Double) -> Ordering
pairSort (x1, y1) (x2, y2) = compare x1 x2

-- average duplicate concentration runlengths
compress :: [(Double, Double)] -> [(Double, Double)]
compress = compress' [] []

compress' :: [(Double, Double)] -> [(Double, Double)] -> [(Double, Double)] -> [(Double, Double)]
compress' zs [] (x:xs) = compress' zs [x] xs
compress' zs ((x2, y2):ys) ((x1,y1):xs) =
    if (x1==x2)
    then compress' zs ((x1, y1):(x2, y2):ys) xs
    else compress' ((runLengthAverage ((x2, y2):ys)):zs) [(x1, y1)] xs
compress' zs ys [] = reverse $ ((runLengthAverage ys):zs)

-- Take a list of pairs of concentration and run
-- return a list of pairs of concentration and ave runlength
-- sorted by concentration
process :: [(Double, Double)] -> [(Double, Double)]
process = process' [] 0.0 []

process' :: [Double] -> Double -> [(Double, Double)] -> [(Double, Double)] -> [(Double, Double)]
process' _ _ ys [] = compress (sortBy pairSort ys)
process' concs runLength ys ((conc, run):xs) = 
              if run==0.0 then
                  if runLength==0.0 then
                      process' [] 0.0 ys xs
                  else
                      process' [] 0.0 (((average concs), runLength*0.02):ys) xs
              else
                 let 
                   concs' = if (concs/=[] && (head concs)==conc) then concs else (conc:concs)
                 in
                   process' concs' (runLength + 1.0) ys xs
                