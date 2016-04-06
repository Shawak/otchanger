function bytes(str)
  if type(str) == 'string' then
    return Encoding.UTF8:GetBytes(str)
  else
    return Convert.ToInt32(str, 16);
  end
end

function toTable(arr)
    local table = {}
    for i = 0, arr.Length - 1 do
      table[i + 1] = arr:GetValue(i)
    end
    return table
end

function getType(name)
  return Type.GetType('System.' .. name)
end