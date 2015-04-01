function string:split(delim)
  local start = 1
  local results = {}
  while true do
    local pos = string.find(self, delim, start, true)
    if not pos then
      break
    end
    table.insert(results, string.sub(self, start, pos-1))
    start = pos + string.len(delim)
  end
  table.insert(results, string.sub(self, start))
  table.removevalue(results, '')
  return results
end

function string:starts(start)
  return string.sub(self, 1, #start) == start
end

function string:ends(test)
   return test =='' or string.sub(self,-string.len(test)) == test
end

function string:trim()
  return string.match(self, '^%s*(.*%S)') or ''
end

function string:explode(sep, limit)
  if type(sep) ~= 'string' or tostring(self):len() == 0 or sep:len() == 0 then
    return {}
  end

  local i, pos, tmp, t = 0, 1, "", {}
  for s, e in function() return string.find(self, sep, pos) end do
    tmp = self:sub(pos, s - 1):trim()
    table.insert(t, tmp)
    pos = e + 1

    i = i + 1
    if limit ~= nil and i == limit then
      break
    end
  end

  tmp = self:sub(pos):trim()
  table.insert(t, tmp)
  return t
end

function string:contains(str, checkCase, start, plain)
  if not checkCase then
    self = self:lower()
    str = str:lower()
  end
  return string.find(self, str, start and start or 1, plain == nil and true or false)
end

function string:replace(pattern, replace, limit)
  return string.gsub(self, pattern, replace, limit)
end

function string:remove(index, count)
  if index < 0 then
    index = #self + index + 1
  end
  return string.sub(self, 0, index - 1) .. string.sub(self, index + count)
end

function string:insert(index, str)
  if index >= 0 then
    return string.sub(self, 0, index) .. str .. string.sub(self, #self - index + 1)
  else
    return string.sub(self, 0, #self + index) .. str .. string.sub(self, #self + index + 1)
  end
end
