import CSV
import System.Environment

main = do
   ~[fname] <- getArgs
   res <- parseFromFile csvfile fname
   case res of Left err -> print err
 	       Right xs -> print xs