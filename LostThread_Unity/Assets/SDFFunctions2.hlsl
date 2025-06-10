// Вместо чтения из текстуры теперь считаем SDF круга в UV

float CircleSDF(float2 uv, float2 center, float radius)
{
    // Расстояние до центра минус радиус
    return length(uv - center) - radius;
}

// Новая версия GetSurfaceNormal_float, которая просто даст нормаль на плоскости круга (вверх)
void GetSurfaceNormal_float(float2 uv, out float3 normal)
{
    // Плоская нормаль, т.к. круг плоский
    normal = float3(0.0, 0.0, 1.0);
}

// Новая ComputeSDF_float с учетом круга
// SSR - screen space ratio (можно оставить как есть)
// SD - signed distance: считаем через CircleSDF
// SDR - signed distance ratio (scale)
// isoPerimeter, softness - параметры сглаживания

void ComputeSDF_float(float SSR, float2 uv, float SDR, float isoPerimeter, float softness, out float outAlpha)
{
    float radius = 0.3; // радиус точки
    float2 center = float2(0.5, 0.5); // центр круга в UV

    float dist = CircleSDF(uv, center, radius); // signed distance круга
    float SD = saturate(0.5 - dist); // нормализуем под 0-1 (примерно)

    softness *= SSR * SDR;
    float d = (SD - 0.5) * SDR;
    outAlpha = saturate((d * 2.0 * SSR + 0.5 + isoPerimeter * SDR * SSR + softness * 0.5) / (1.0 + softness));
}
