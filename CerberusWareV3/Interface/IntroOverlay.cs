using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using CerberusWareV3.MyImGui;
using Hexa.NET.ImGui;

[CompilerGenerated]
public class IntroOverlay : IOverlay
{
    // --- Настройки (Configuration) ---
    private const float AnimationDelay = 20.0f;      // Задержка перед стартом (20 секунд)
    private const float TextHoldDuration = 8.0f;     // Сколько секунд текст висит неподвижно (8 секунд)
    private const float ParticleBaseSpeed = 6.0f;    // Скорость полета частиц
    private const float MouseRepulsionRadius = 150f; // Радиус отталкивания от мыши
    private const float MouseRepulsionForce = 500f;  // Сила отталкивания
    private const float ParticleSize = 3.5f;         // Размер частиц (сделал побольше для читаемости)

    // --- Переменные состояния ---
    private bool _initialized = false;
    private bool _animationStarted = false;
    private bool _skipped = false;
    private float _timeElapsed = 0f;
    private Vector2 _screenSize;
    
    private readonly List<Particle> _particles = new List<Particle>();
    private readonly List<Vector2> _targets = new List<Vector2>();
    private Dictionary<char, string[]> _fontMap;
    private readonly Random _random = new Random();

    private enum IntroState
    {
        Waiting,    // Ждем 20 сек
        Explosion,  // Хаотичный разлет
        Gathering,  // Сборка в текст
        Holding,    // Показ текста
        Dispersing  // Финал
    }
    private IntroState _currentState = IntroState.Waiting;

    public unsafe void Render()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        _screenSize = io.DisplaySize;

        if (!_initialized)
        {
            Initialize();
        }

        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(_screenSize);
        ImGui.Begin("##IntroCanvas", 
            ImGuiWindowFlags.NoTitleBar | 
            ImGuiWindowFlags.NoResize | 
            ImGuiWindowFlags.NoMove | 
            ImGuiWindowFlags.NoScrollbar | 
            ImGuiWindowFlags.NoCollapse | 
            ImGuiWindowFlags.NoBackground | 
            ImGuiWindowFlags.NoSavedSettings | 
            ImGuiWindowFlags.NoInputs);

        // Пропуск по клику
        if (ImGui.IsMouseClicked(ImGuiMouseButton.Left) || ImGui.IsMouseClicked(ImGuiMouseButton.Right))
        {
            if (!_skipped)
            {
                _skipped = true;
                _currentState = IntroState.Dispersing;
            }
        }

        UpdateAndDraw(io.DeltaTime);

        ImGui.End();

        // Удаление оверлея после окончания
        if (_currentState == IntroState.Dispersing && _particles.All(p => p.Alpha <= 0.01f))
        {
             RenderManager.Instance.UnregisterRender(this);
        }
    }

    private void Initialize()
    {
        InitializeFont();
        
        // Текст
        string line1 = "By DobriyKaban";
        string line2 = "for t.me/RobusterHome";
        
        GenerateTextTargets(line1, line2);
        
        // Количество частиц
        int particleCount = Math.Max(3000, _targets.Count * 1);
        InitializeParticles(particleCount);

        _initialized = true;
    }

    private void UpdateAndDraw(float deltaTime)
    {
        _timeElapsed += deltaTime;
        ImDrawListPtr drawList = ImGui.GetWindowDrawList();
        Vector2 mousePos = ImGui.GetMousePos();

        // 1. Логика таймера запуска
        if (!_animationStarted && _timeElapsed >= AnimationDelay)
        {
            _animationStarted = true;
            _currentState = IntroState.Explosion;
            _timeElapsed = 0; // Сбрасываем таймер для удобства фаз анимации
        }

        // 2. Логика переключения фаз (если анимация началась)
        if (_animationStarted)
        {
            float gatheringStart = 1.5f; 
            float holdingStart = gatheringStart + 4.0f; // Даем 4 секунды на плавную сборку
            float dispersingStart = holdingStart + TextHoldDuration;

            switch (_currentState)
            {
                case IntroState.Explosion:
                    if (_timeElapsed > gatheringStart) _currentState = IntroState.Gathering;
                    break;
                case IntroState.Gathering:
                    if (_timeElapsed > holdingStart) _currentState = IntroState.Holding;
                    break;
                case IntroState.Holding:
                    if (_timeElapsed > dispersingStart) _currentState = IntroState.Dispersing;
                    break;
            }
        }

        // 3. Обновление частиц
        for (int i = 0; i < _particles.Count; i++)
        {
            var p = _particles[i];
            
            Vector2 targetPos = p.Position;
            float activeSpeed = ParticleBaseSpeed;

            if (_currentState == IntroState.Waiting)
            {
                // Пока ждем - легкое покачивание на месте
                targetPos = p.Position + new Vector2((float)Math.Sin(_timeElapsed + p.NoiseOffset), (float)Math.Cos(_timeElapsed + p.NoiseOffset)) * 0.5f;
                activeSpeed = 0.5f;
            }
            else if (_currentState == IntroState.Explosion)
            {
                // Разлет из центра
                Vector2 center = new Vector2(_screenSize.X / 2, _screenSize.Y / 2);
                Vector2 dir = p.Position - center;
                targetPos = p.Position + Vector2.Normalize(dir) * 500f;
                activeSpeed = 3.0f;
            }
            else if (i < _targets.Count && (_currentState == IntroState.Gathering || _currentState == IntroState.Holding))
            {
                // Это частица текста - летит к букве
                targetPos = _targets[i];
                
                // В фазе Holding добавляем микро-дрожание, чтобы текст был "живым", но читаемым
                if (_currentState == IntroState.Holding)
                {
                    float wobble = (float)Math.Sin(_timeElapsed * 5f + p.NoiseOffset) * 1.5f;
                    targetPos += new Vector2(wobble, wobble);
                    activeSpeed = 15.0f; // Очень сильное притяжение, чтобы держать форму
                }
                else
                {
                    activeSpeed = 8.0f; // Скорость сборки
                }
            }
            else if (_currentState == IntroState.Dispersing)
            {
                // Разлет в конце
                Vector2 dir = p.Position - new Vector2(_screenSize.X / 2, _screenSize.Y / 2);
                targetPos = p.Position + Vector2.Normalize(dir) * 2000f;
                activeSpeed = 8.0f;
                p.Alpha -= deltaTime * 0.5f;
            }
            else
            {
                // Фоновые частицы (не вошли в текст)
                targetPos = new Vector2(
                    _screenSize.X / 2 + (float)Math.Sin(_timeElapsed * 0.5f + p.NoiseOffset) * _screenSize.X * 0.5f,
                    _screenSize.Y / 2 + (float)Math.Cos(_timeElapsed * 0.3f + p.NoiseOffset) * _screenSize.Y * 0.5f
                );
            }

            // Физика движения
            Vector2 toTarget = targetPos - p.Position;
            
            // Отталкивание от мыши (кроме фазы ожидания и конца)
            if (_currentState != IntroState.Waiting && _currentState != IntroState.Dispersing)
            {
                float distToMouse = Vector2.Distance(p.Position, mousePos);
                if (distToMouse < MouseRepulsionRadius)
                {
                    Vector2 repulsion = Vector2.Normalize(p.Position - mousePos) * ((MouseRepulsionRadius - distToMouse) / MouseRepulsionRadius) * MouseRepulsionForce;
                    p.Velocity += repulsion * deltaTime;
                }
            }

            p.Velocity += toTarget * activeSpeed * deltaTime;
            p.Velocity *= 0.92f; // Сопротивление воздуха (плавность)
            p.Position += p.Velocity * deltaTime;

            // Отрисовка
            if (p.Alpha > 0.05f)
            {
                // Цвета: Яркий Cyan слева -> Яркая Magenta справа
                float t = Math.Clamp(p.Position.X / _screenSize.X, 0f, 1f);
                
                // Используем максимально яркие цвета (RGB 0..1)
                Vector4 col1 = new Vector4(0.0f, 1.0f, 1.0f, p.Alpha); // Cyan
                Vector4 col2 = new Vector4(1.0f, 0.2f, 1.0f, p.Alpha); // Neon Pink
                
                Vector4 finalColor = Vector4.Lerp(col1, col2, t);
                uint uColor = ImGui.ColorConvertFloat4ToU32(finalColor);

                drawList.AddCircleFilled(p.Position, ParticleSize, uColor);
            }

            _particles[i] = p;
        }
    }

    private void GenerateTextTargets(string text1, string text2)
    {
        _targets.Clear();
        // Увеличил масштаб для лучшей читаемости
        float scale = 5.0f; 
        float spacing = 2.0f * scale;

        void ProcessLine(string text, float yOffset)
        {
            float totalWidth = 0;
            foreach (char c in text)
            {
                char upper = char.ToUpper(c);
                if (_fontMap.ContainsKey(upper)) totalWidth += (5 * scale) + spacing;
                else if (c == ' ') totalWidth += (3 * scale) + spacing;
                else if (_fontMap.ContainsKey(c)) totalWidth += (5 * scale) + spacing;
            }

            float currentX = (_screenSize.X - totalWidth) / 2;

            foreach (char c in text)
            {
                char key = char.ToUpper(c);
                if (!_fontMap.ContainsKey(key) && _fontMap.ContainsKey(c)) key = c;

                if (c == ' ')
                {
                    currentX += (3 * scale) + spacing;
                    continue;
                }

                if (_fontMap.ContainsKey(key))
                {
                    string[] mask = _fontMap[key];
                    for (int row = 0; row < mask.Length; row++)
                    {
                        for (int col = 0; col < mask[row].Length; col++)
                        {
                            if (mask[row][col] == '1')
                            {
                                Vector2 point = new Vector2(currentX + (col * scale), yOffset + (row * scale));
                                _targets.Add(point);
                            }
                        }
                    }
                    currentX += (mask[0].Length * scale) + spacing;
                }
                else
                {
                    currentX += (5 * scale) + spacing;
                }
            }
        }

        float centerY = _screenSize.Y / 2;
        ProcessLine(text1, centerY - 60f); 
        ProcessLine(text2, centerY + 30f);
    }

    private void InitializeParticles(int count)
    {
        _particles.Clear();
        for (int i = 0; i < count; i++)
        {
            _particles.Add(new Particle
            {
                Position = new Vector2((float)_random.NextDouble() * _screenSize.X, (float)_random.NextDouble() * _screenSize.Y),
                Velocity = new Vector2(((float)_random.NextDouble() - 0.5f) * 10f, ((float)_random.NextDouble() - 0.5f) * 10f),
                NoiseOffset = (float)_random.NextDouble() * 100f,
                Alpha = 1.0f
            });
        }
    }

    private void InitializeFont()
    {
        _fontMap = new Dictionary<char, string[]>();
        // Буквы A-Z
        _fontMap['A'] = new[] { "01110", "10001", "10001", "10001", "11111", "10001", "10001" };
        _fontMap['B'] = new[] { "11110", "10001", "10001", "11110", "10001", "10001", "11110" };
        _fontMap['C'] = new[] { "01110", "10001", "10000", "10000", "10000", "10001", "01110" };
        _fontMap['D'] = new[] { "11110", "10001", "10001", "10001", "10001", "10001", "11110" };
        _fontMap['E'] = new[] { "11111", "10000", "10000", "11110", "10000", "10000", "11111" };
        _fontMap['F'] = new[] { "11111", "10000", "10000", "11110", "10000", "10000", "10000" };
        _fontMap['G'] = new[] { "01110", "10001", "10000", "10011", "10001", "10001", "01110" };
        _fontMap['H'] = new[] { "10001", "10001", "10001", "11111", "10001", "10001", "10001" };
        _fontMap['I'] = new[] { "01110", "00100", "00100", "00100", "00100", "00100", "01110" };
        _fontMap['J'] = new[] { "00111", "00001", "00001", "00001", "10001", "10001", "01110" };
        _fontMap['K'] = new[] { "10001", "10010", "10100", "11000", "10100", "10010", "10001" };
        _fontMap['L'] = new[] { "10000", "10000", "10000", "10000", "10000", "10000", "11111" };
        _fontMap['M'] = new[] { "10001", "11011", "10101", "10101", "10001", "10001", "10001" };
        _fontMap['N'] = new[] { "10001", "11001", "10101", "10011", "10001", "10001", "10001" };
        _fontMap['O'] = new[] { "01110", "10001", "10001", "10001", "10001", "10001", "01110" };
        _fontMap['P'] = new[] { "11110", "10001", "10001", "11110", "10000", "10000", "10000" };
        _fontMap['Q'] = new[] { "01110", "10001", "10001", "10001", "10001", "10010", "01101" };
        _fontMap['R'] = new[] { "11110", "10001", "10001", "11110", "10100", "10010", "10001" };
        _fontMap['S'] = new[] { "01111", "10000", "10000", "01110", "00001", "00001", "11110" };
        _fontMap['T'] = new[] { "11111", "00100", "00100", "00100", "00100", "00100", "00100" };
        _fontMap['U'] = new[] { "10001", "10001", "10001", "10001", "10001", "10001", "01110" };
        _fontMap['V'] = new[] { "10001", "10001", "10001", "10001", "10001", "01010", "00100" };
        _fontMap['W'] = new[] { "10001", "10001", "10001", "10101", "10101", "10101", "01010" };
        _fontMap['X'] = new[] { "10001", "10001", "01010", "00100", "01010", "10001", "10001" };
        _fontMap['Y'] = new[] { "10001", "10001", "10001", "01010", "00100", "00100", "00100" };
        _fontMap['Z'] = new[] { "11111", "00001", "00010", "00100", "00100", "01000", "11111" };
        // Символы
        _fontMap['.'] = new[] { "000", "000", "000", "000", "000", "000", "010" };
        _fontMap['/'] = new[] { "00001", "00001", "00010", "00100", "01000", "10000", "10000" };
        _fontMap['-'] = new[] { "00000", "00000", "00000", "11111", "00000", "00000", "00000" };
    }

    private struct Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float NoiseOffset;
        public float Alpha;
    }
}