-- CSV.hs Haskell module for parsing CSV (comma separated value) files.
-- Copyright (c) Bryn Keller 2002. Released under GNU LGPL.

module CSV (csvfile, csvline) where

import Text.ParserCombinators.Parsec
import System.Environment (getArgs)
import Data.List (nubBy)

csvfile = many csvline

csvline = do { cells <- (sepBy cell comma); newline; return cells }

comma = char ','

cell = do
  many (char ' ')
  lquotes <- many (char '"')
  let quoteLen = length lquotes
  raw_text <- manyTill anyChar (try (count quoteLen (char '"')))
  let clean_text = if quoteLen > 0 then cleanQuotes raw_text else raw_text
  many (char ' ')
  return clean_text
  where			  
    cleanQuotes s = nubBy (\x y -> x == '"' && y == '"') s

-- Usage:
-- main = do
--   ~[fname] <- getArgs
--   res <- parseFromFile csvfile fname
--   case res of Left err -> print err
-- 	      Right xs -> print xs