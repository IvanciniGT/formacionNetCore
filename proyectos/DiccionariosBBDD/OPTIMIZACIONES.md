# Optimizaciones de Rendimiento en DiccionariosBBDD

## Problema Identificado

**❌ ANTES - Ineficiente:**
```csharp
// Aplicaba UPPER() a la columna SIN índice funcional
var existe = _context.Idiomas
    .Any(i => i.Codigo.ToUpper() == codigoIdioma);
```

**SQL generado (INEFICIENTE):**
```sql
SELECT 1 FROM Idiomas 
WHERE UPPER(Codigo) = 'ES'
```
- **Problema:** `UPPER()` aplicado a la columna causa **full table scan** sin índice funcional
- **Resultado:** No puede usar índices, muy lento con muchos datos

## Solución Implementada: Índices Funcionales

**✅ DESPUÉS - Optimizado con Índices Funcionales:**
```csharp
// Crear índices funcionales primero
_context.CreateFunctionalIndexes();

// Luego usar UPPER() en queries para aprovechar el índice
var codigoIdioma = idioma.ToUpperInvariant();
var existe = _context.Idiomas
    .Any(i => i.Codigo.ToUpper() == codigoIdioma);
```

**SQL generado (ÓPTIMO):**
```sql
-- Índice funcional creado previamente:
CREATE INDEX IX_Idiomas_Codigo_Upper ON Idiomas (UPPER(Codigo))

-- Query que USA el índice funcional:
SELECT 1 FROM Idiomas 
WHERE UPPER(Codigo) = 'ES'
```
- **Ventaja:** `UPPER()` aplicado a la columna **USA el índice funcional**
- **Resultado:** Búsqueda instantánea incluso con millones de registros

## Estrategia de Índices Funcionales

### Datos de Entrada
Los datos pueden almacenarse en **cualquier formato** (mayúsculas, minúsculas, mixto):
- `"es"`, `"ES"`, `"Es"` → Todos funcionan
- `"En"`, `"en"`, `"EN"` → Todos funcionan  
- `"fr"`, `"Fr"`, `"FR"` → Todos funcionan

### Índices Funcionales Creados
```csharp
public void CreateFunctionalIndexes()
{
    // Índice funcional para códigos de idioma
    Database.ExecuteSqlRaw("CREATE INDEX IF NOT EXISTS IX_Idiomas_Codigo_Upper ON Idiomas (UPPER(Codigo))");
    
    // Índice funcional para palabras (elimina necesidad de TextoNormalizado)
    Database.ExecuteSqlRaw("CREATE INDEX IF NOT EXISTS IX_Palabras_Texto_Upper ON Palabras (UPPER(Texto))");
}
```

### Búsquedas Optimizadas
Las búsquedas usan `UPPER()` para aprovechar los índices funcionales:
```csharp
var codigoIdioma = idioma.ToUpperInvariant(); // "es" → "ES"
// ✅ Usa el índice funcional IX_Idiomas_Codigo_Upper
.Any(i => i.Codigo.ToUpper() == codigoIdioma)
```

**SQL generado que SÍ usa índice funcional:**
```sql
-- ✅ Esta query usa el índice funcional IX_Idiomas_Codigo_Upper
SELECT * FROM Idiomas WHERE UPPER(Codigo) = 'ES'
```

## Campo TextoNormalizado Eliminado

**❌ ANTES - Redundante:**
```csharp
public class PalabraEntity
{
    public string Texto { get; set; }
    public string TextoNormalizado { get; set; } // ❌ Redundante!
}

// Query que usa campo duplicado
.Where(p => p.TextoNormalizado == palabraNormalizada)
```

**✅ DESPUÉS - Limpio:**
```csharp
public class PalabraEntity
{
    public string Texto { get; set; }
    // TextoNormalizado eliminado - usamos índice funcional
}

// Query que usa índice funcional
.Where(p => p.Texto.ToUpper() == palabraNormalizada)
```

## Lección Aprendida

> **Regla de Oro:** Los índices funcionales eliminan la necesidad de campos normalizados redundantes. Si tienes `CREATE INDEX IX_Tabla_Campo_Upper ON Tabla (UPPER(Campo))`, entonces usa `WHERE UPPER(Campo) = valor` en las queries.

### Opciones para búsquedas case-insensitive (en orden de preferencia):

1. **✅ ÓPTIMO:** Índice funcional + query con función (sin campos redundantes)
2. **✅ BUENO:** Normalizar datos al insertar + comparación directa  
3. **❌ MALO:** Aplicar función a columna sin índice funcional
4. **❌ PEOR:** Columna computada/normalizada + trigger (complejidad y duplicidad)

## Rendimiento

- **Antes (sin índice funcional):** O(n) full table scan
- **Después (con índice funcional):** O(log n) búsqueda por índice
- **Mejora:** De millisegundos a microsegundos con grandes volúmenes de datos
- **Ventaja adicional:** Sin campos redundantes, esquema más limpio