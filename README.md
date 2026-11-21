# Yvsim Extensions

Unity 常用扩展方法集合，涵盖集合、字符串、Transform、层级、组件、包围盒、点击回调、Vector、Color、LayerMask，以及 UGUI 的 RectTransform、布局重建、UI 事件和 TextMesh Pro 事件。

所有扩展方法统一使用 `Yv`（Yvsim）前缀，与 Unity 和项目自身的普通方法明确区分，在 IntelliSense 中输入 `Yv` 即可列出全部 Yvsim 扩展；全部类型位于 `Yvsim.Extensions` 命名空间。

## 结构

包按“独立扩展 + 组合扩展”组织：

| 位置 | 内容 | 规则 |
| --- | --- | --- |
| `Runtime/Independent/` | 独立扩展 | 每个类只覆盖一个功能范围；每个方法功能唯一、完整、独立、闭合，只依赖 Unity API，不调用其他扩展方法（同一方法的重载转发和递归除外） |
| `Runtime/*CompositeExtensions.cs` | 组合扩展 | 由多个独立扩展组合而成，只依赖 Unity 内置模块 |
| `Samples~/` | 依赖第三方插件的独立扩展与组合扩展 | 先安装对应插件，再导入 Sample |

| 独立扩展类 | 功能范围 |
| --- | --- |
| `CollectionExtensions` | 集合判空、安全索引、随机、打乱、权重随机、日志输出 |
| `StringExtensions` | 缩进、控制字符过滤、Unity 富文本 |
| `TransformExtensions` | 单个 Transform 的位置、旋转、缩放：重置、单轴赋值、复制、水平朝向；不涉及父子层级 |
| `HierarchyExtensions` | 父子层级结构：递归查找子节点、层级路径、清空直接子物体 |
| `ActiveStateExtensions` | 激活状态：设置、切换自身及直接子物体的 activeSelf |
| `ObjectLifetimeExtensions` | 对象生命周期：运行模式下安全销毁 `UnityEngine.Object` |
| `ComponentExtensions` | 组件：获取或添加、必需组件、父子级查找、批量添加和移除 |
| `BoundsExtensions` | 包围盒：Renderer 世界包围盒、层级局部包围盒 |
| `LayerExtensions` | Layer：LayerMask 判断、添加、移除，以及层级递归设置 Layer |
| `GameObjectClickExtensions` | 点击：场景物体左键点击回调 |
| `VectorExtensions` / `ColorExtensions` | 值类型：Vector 单轴操作；颜色 Alpha、十六进制转换与解析 |
| `RectTransformExtensions` | UI 矩形：获取 RectTransform、锚点坐标与尺寸单轴赋值、保持视觉位置修改 Pivot |
| `GraphicExtensions` | UI 图形：修改 Image、RawImage、Text 等 Graphic 的透明度 |
| `CanvasGroupExtensions` | CanvasGroup：一次设置透明度、交互和射线拦截 |
| `SelectableExtensions` | Selectable：关闭键盘和手柄导航 |
| `LayoutExtensions` | UGUI 布局：立即重建布局树、下一帧重建最近的布局节点（刷新 ScrollRect 内容等） |
| `UIEventExtensions` | UI 事件：以 `System.Action` 设置 UGUI 组件、EventTrigger 和 TMP 组件的事件监听 |

| 组合扩展 | 组合方式 |
| --- | --- |
| `GameObjectCompositeExtensions.YvAddFittedBoxCollider` | `YvTryGetHierarchyLocalBounds` + `YvGetOrAddComponent` |
| `TransformCompositeExtensions.YvSetParentAndResetLocal` | 设置父节点 + `YvResetLocal` |
| `TextCompositeExtensions.YvSetText` | 设置 Text / TMP_Text 文本 + `YvSetActive`（按内容显隐） |

TextMesh Pro 为可选依赖：项目包含 `com.unity.textmeshpro`（或 Unity 6 中内置 TMP 的 `com.unity.ugui` 2.x）时自动定义 `YVSIM_TMP`，`TMP_InputField`、`TMP_Dropdown` 事件扩展和 `TMP_Text.YvSetText` 才参与编译；没有 TMP 的项目也能正常使用本包。

## 安装

当前源码版本为 `1.0.0`。准备本包源码，使用本地 UPM 安装：

1. 在 `Window > Package Manager > + > Add package from disk...` 选择本包根目录的 `package.json`。
2. 等待 Unity 解析清单中声明的 Unity 依赖并完成编译。

也可以将完整包目录放入项目的 `Packages/com.yvsim.extensions`，作为 Embedded Package 使用。

本包提供本地源码、Git 和 OpenUPM 安装方式；固定版本使用 `1.0.0`，具体步骤见下文。

## 使用方法

在使用扩展的脚本中添加命名空间：

```csharp
using Yvsim.Extensions;
```

### 集合日志和随机取值

```csharp
using System.Collections.Generic;

List<string> names = new List<string> { "A", "B", "C" };

names.YvLogItems();
string randomName = names.YvGetRandomItem();
int randomIndex = names.YvGetRandomIndex();
names.YvShuffle();
string removedName = names.YvPopRandom();
```

### 字符串处理

```csharp
string text = "第一行\n第二行";

string indented = text.YvIndentLines();
string cleanText = text.YvRemoveControlCharacters();
string compactText = text.YvRemoveWhitespaceAndControlCharacters();
```

`YvIndentLines` 默认使用两个全角空格缩进，并包含第一行。可以自定义：

```csharp
string result = text.YvIndentLines(
    includeFirstLine: false,
    indent: "    ");
```

### 字符串富文本

```csharp
string bold = "重点".YvRichBold();
string italic = "说明".YvRichItalic();
string large = "标题".YvRichSize(36);
string red = "警告".YvRichColor("red");
string translucent = "提示".YvRichColor(new Color(1f, 0.5f, 0f, 0.8f));

string combined = "重要提示"
    .YvRichBold()
    .YvRichSize(32)
    .YvRichColor(Color.yellow);
```

这些方法只生成 Unity 富文本标签，不依赖 UGUI 或 TMP 组件。显示端仍需启用 Rich Text。`YvRichColor(Color)` 默认包含 Alpha，可通过 `includeAlpha: false` 只输出 RGB。

### Unity 对象操作

```csharp
targetComponent.YvSetActive(true);

targetTransform.YvCopyPoseFrom(referenceTransform);
targetTransform.YvCopyPositionFrom(referenceTransform);
targetTransform.YvCopyRotationFrom(referenceTransform);
targetTransform.YvLookAtHorizontal(referenceTransform);
targetTransform.YvResetLocal();
targetTransform.gameObject.YvSetLayerRecursive(7);
Transform child = targetTransform.YvFindChildRecursive("Target");
string hierarchyPath = targetTransform.YvGetHierarchyPath();
```

### 获取或添加组件

```csharp
AudioSource audioSource = gameObject.YvGetOrAddComponent<AudioSource>();
Rigidbody body = transform.YvGetOrAddComponent<Rigidbody>();
```

目标为 null 时返回 null；目标已包含组件时直接返回已有组件。

### 自动包裹层级的 BoxCollider

```csharp
BoxCollider collider = gameObject.YvAddFittedBoxCollider();
BoxCollider activeOnlyCollider = transform.YvAddFittedBoxCollider(
    includeInactive: false);
```

该方法会在调用时计算目标及全部子物体的 Renderer 和子级 3D Collider，
将结果转换为目标物体的局部包围盒，然后添加或更新目标上的 BoxCollider。
默认包含未激活子物体；目标已有的 BoxCollider 会被复用，子碰撞体不会被删除。
这是运行时按需计算方法，层级或模型发生变化后需要再次调用，不会每帧自动更新。

`YvAddFittedBoxCollider` 是组合扩展，由独立扩展 `YvTryGetHierarchyLocalBounds` 和 `YvGetOrAddComponent` 组合而成。只需要包围盒时可以直接使用前者：

```csharp
if (transform.YvTryGetHierarchyLocalBounds(out Bounds localBounds))
{
    Debug.Log(localBounds.size);
}
```

### 点击回调

```csharp
target.YvSetClickAction(() => Debug.Log("每次左键点击都会触发"));
target.YvSetClickAction(() => Debug.Log("替换为新的回调"));
target.YvSetClickAction(null); // 取消监听
```

点击检测基于 `IPointerClickHandler`：3D 物体需要 Collider，场景需要 EventSystem 和带 PhysicsRaycaster 的相机；UI 物体需要 GraphicRaycaster。

### UGUI 矩形、透明度和交互

```csharp
RectTransform buttonRect = button.YvGetRect();
image.YvSetAlpha(0.5f);
panel.YvSetAnchoredPositionX(100f);
panel.YvSetSizeDeltaY(300f);
panel.YvSetPivotKeepPosition(new Vector2(0.5f, 1f));
canvasGroup.YvSetCanvasGroupState(0f, false, false);
button.YvSetNavigationNone();
label.YvSetText(message, hideWhenEmpty: true);
```

`YvGetRect` 可由 Button、Image、TMP_Text、Slider、Toggle 等 UI 组件直接调用，目标为空或不是 RectTransform 时返回 `null`。`YvSetAlpha` 保留原有 RGB，并把透明度限制在 `0–1`。

### 刷新布局（ScrollRect 等）

```csharp
content.YvRebuildLayoutTree();                 // 立即按先子后父的顺序重建整个布局树
content.YvRebuildNearestLayoutNextFrame(this); // 下一帧重建最近的 LayoutGroup / ContentSizeFitter
```

动态增删 ScrollRect 内容后需要立刻得到正确尺寸时使用 `YvRebuildLayoutTree`；本帧尺寸尚未确定（例如刚修改文本）时使用 `YvRebuildNearestLayoutNextFrame`。延迟重建需要一个启动协程的 `MonoBehaviour`，不传时从目标父级自动查找；目标在下一帧前被销毁时不执行重建。

### UI 事件

```csharp
button.YvSetOnClick(OnStartButtonClick);
toggle.YvSetOnValueChanged(OnToggleChanged);
slider.YvSetOnValueChanged(OnSliderChanged);
scrollRect.YvSetOnValueChanged(OnScrollPositionChanged);
inputField.YvSetOnEndEdit(OnInputEndEdit);
eventTrigger.YvSetEventTrigger(EventTriggerType.PointerEnter, OnPointerEnter);

tmpInputField.YvSetOnSubmit(OnInputSubmitted);   // 项目包含 TextMesh Pro 时可用
tmpDropdown.YvSetOnValueChanged(OnDropdownChanged);

button.YvSetOnClick(OnAdditionalClick, false);   // 保留已有监听并追加
```

事件扩展默认先移除代码注册的全部运行时监听，再添加新回调；最后一个参数传入 `false` 时保留已有监听并追加。Inspector 中配置的持久化监听不会被移除。覆盖 `Button`、`Toggle`、`InputField`、`Dropdown`、`Slider`、`Scrollbar`、`ScrollRect`、`MaskableGraphic`、`EventTrigger`、`TMP_InputField` 和 `TMP_Dropdown`。`InputField.YvSetOnSubmit` 绑定到 `onSubmit`。

`YvSetOnClick` 绑定的是 `Button.onClick`，遵守 Button 的 interactable 状态并响应键盘提交；`YvSetClickAction` 用于没有 Button 的场景物体。

## Highlight Plus Integration Sample

Package Manager 的 Samples 区域提供：

```text
Highlight Plus Integration
```

导入前必须先安装 Highlight Plus。

示例同样按“独立扩展 + 组合扩展”组织：`Independent` 目录存放依赖 Highlight Plus 的独立扩展，根目录的组合扩展把它与包内的点击扩展拼装在一起。

| 扩展方法 | 所在类 | 位置 | 说明 |
| --- | --- | --- | --- |
| `YvSetClickAction` | `GameObjectClickExtensions` | 包内 Runtime | 设置每次左键点击时的回调；重复设置时替换，传入 null 时取消监听 |
| `YvSetHighlight` | `GameObjectHighlightExtensions` | Sample | 开启或取消高亮，可选实体提亮、外发光和统一颜色；开启时描边带呼吸效果，取消时立即关闭 |
| `YvSetHighlightClick` | `GameObjectHighlightCompositeExtensions` | Sample | 高亮并等待一次点击，点击后自动取消高亮和监听，再执行回调 |

```csharp
// 独立使用
target.YvSetHighlight(true, includeGlow: true);
target.YvSetClickAction(() => Debug.Log("每次点击都会触发"));

// 组合使用：高亮提示，点击一次后自动清理
target.YvSetHighlightClick(() => Debug.Log("目标被点击"));

// 取消
target.YvSetHighlightClick(null);
```

点击检测基于 `IPointerClickHandler`：3D 物体需要 Collider，场景需要 EventSystem 和带 PhysicsRaycaster 的相机。目标为 null 时所有方法都不执行任何操作。

Sample 未导入时不会参与编译，也不会要求项目安装 Highlight Plus。

## 注意事项

- **删除类方法只在运行模式下生效**：`YvDestroy`、`YvRemoveComponent`、`YvClearChildren` 在编辑模式下不做任何修改；销毁在帧末生效。
- **值类型需要接收返回值**：Vector、Color、LayerMask 的扩展返回新值，例如 `position = position.YvWithY(0f)`、`mask = mask.YvAddLayer(8)`。
- **UI 事件扩展默认替换监听**：会清掉该事件上所有通过代码注册的监听（包括其他脚本注册的），需要保留时把最后一个参数设为 `false`；回调不能用 `RemoveListener` 单独移除，取消时传入 `null`。
- **会修改原集合的方法**：`YvShuffle`、`YvPopRandom`、`YvRemoveNullObjects` 直接修改传入的列表，后两者不能用于数组。
- **随机方法只能在主线程调用**：它们使用 `UnityEngine.Random`。
- **目标为 null 时安全返回**：除 `YvGetComponentOrThrow` 外，扩展方法遇到 null 目标都不会抛出异常，而是直接返回或返回默认值。

每个方法的完整用法、参数含义和使用限制，请查看 IDE 中的方法注释。

## API 速查

### CollectionExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `IReadOnlyCollection<T>` | `YvIsNullOrEmpty<T>()` | 判断集合是否为 null 或不包含任何元素。 |
| `IReadOnlyList<T>` | `YvGetOrDefault<T>(index, defaultValue)` | 按索引安全读取元素，列表为 null 或索引越界时不抛异常，返回 defaultValue。 |
| `IReadOnlyList<T>` | `YvLogItems<T>(context)` | 用一条 Debug.Log 输出列表的元素数量，以及每个元素的索引和 ToString() 内容，便于调试。 |
| `IReadOnlyList<T>` | `YvGetRandomItem<T>()` | 等概率随机返回列表中的一个元素，不修改列表。 |
| `IReadOnlyList<T>` | `YvGetRandomIndex<T>()` | 等概率随机返回列表的一个有效索引。 |
| `IList<T>` | `YvShuffle<T>()` | 用 Fisher-Yates 算法原地随机打乱元素顺序，每种排列概率相同。 |
| `IList<T>` | `YvPopRandom<T>()` | 随机取出一个元素并从列表中删除，可用于不重复抽取。 |
| `IReadOnlyList<T>` | `YvGetWeightedRandom<T>(weightSelector)` | 按权重随机返回一个元素，权重越大被选中的概率越高，不修改列表。 |
| `IList<T>` | `YvRemoveNullObjects<T>()` | 从列表中删除 null 和已被 Destroy 的 Unity 对象，保留其余元素的顺序。 |

### StringExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `string` | `YvIndentLines(includeFirstLine, indent)` | 在每一行开头插入缩进，常用于中文段落首行缩进。 |
| `string` | `YvRemoveControlCharacters(preserveFormattingWhitespace)` | 删除不可见的控制字符（如 \0、\a 等 char.IsControl 为 true 的字符），普通空格保留。常用于清理外部导入或粘贴的文本。 |
| `string` | `YvRemoveWhitespaceAndControlCharacters()` | 删除全部空白字符（包括空格、全角空格、制表符和换行）和控制字符，常用于比较或校验输入内容。 |
| `string` | `YvRichBold()` | 用 <b> 标签包裹文本，显示为粗体。可与其他富文本方法链式组合。 |
| `string` | `YvRichItalic()` | 用 <i> 标签包裹文本，显示为斜体。可与其他富文本方法链式组合。 |
| `string` | `YvRichSize(fontSize)` | 用 <size> 标签包裹文本，单独设置这段文本的字号。可与其他富文本方法链式组合。 |
| `string` | `YvRichColor(color)` | 用 <color> 标签包裹文本，设置这段文本的颜色。可与其他富文本方法链式组合。 |
| `string` | `YvRichColor(color, includeAlpha)` | 用 <color> 标签包裹文本，颜色由 UnityEngine.Color 转成十六进制值。可与其他富文本方法链式组合。 |

### TransformExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Transform` | `YvResetLocalPosition()` | 把本地位置设为 (0, 0, 0)，使物体与父节点原点重合；没有父节点时即世界原点。 |
| `Transform` | `YvResetLocalRotation()` | 把本地旋转设为无旋转（Quaternion.identity），使朝向与父节点一致。 |
| `Transform` | `YvResetLocalScale()` | 把本地缩放设为 (1, 1, 1)。 |
| `Transform` | `YvResetLocal()` | 把本地位置、旋转、缩放全部重置为默认值（0、无旋转、1），常用于把物体对齐到父节点。 |
| `Transform` | `YvCopyLocalTransformFrom(reference)` | 把 reference 的本地位置、旋转、缩放数值复制给 target。 |
| `Transform` | `YvCopyPoseFrom(reference)` | 把 target 移动并旋转到 reference 的世界位置和世界旋转，缩放不变。 |
| `Transform` | `YvCopyPositionFrom(reference)` | 把 target 移到 reference 的世界位置，旋转和缩放不变。 |
| `Transform` | `YvCopyRotationFrom(reference)` | 把 target 的世界旋转设为与 reference 相同，位置和缩放不变。 |
| `Transform` | `YvLookAtHorizontal(reference)` | 让 target 在水平面上朝向 reference（只绕世界 Y 轴转动，不抬头低头），常用于角色或 NPC 转身面向目标。 |
| `Transform` | `YvSetPositionX(value)` | 只修改世界坐标的 X，其余分量不变。 |
| `Transform` | `YvSetPositionY(value)` | 只修改世界坐标的 Y，其余分量不变。 |
| `Transform` | `YvSetPositionZ(value)` | 只修改世界坐标的 Z，其余分量不变。 |
| `Transform` | `YvSetLocalPositionX(value)` | 只修改本地坐标（相对父节点）的 X，其余分量不变。 |
| `Transform` | `YvSetLocalPositionY(value)` | 只修改本地坐标（相对父节点）的 Y，其余分量不变。 |
| `Transform` | `YvSetLocalPositionZ(value)` | 只修改本地坐标（相对父节点）的 Z，其余分量不变。 |
| `Transform` | `YvSetLocalScaleX(value)` | 只修改本地缩放的 X，其余分量不变。 |
| `Transform` | `YvSetLocalScaleY(value)` | 只修改本地缩放的 Y，其余分量不变。 |
| `Transform` | `YvSetLocalScaleZ(value)` | 只修改本地缩放的 Z，其余分量不变。 |

### HierarchyExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Transform` | `YvFindChildRecursive(childName, includeSelf)` | 按名称在全部子孙节点中深度优先查找第一个匹配的节点，未激活的物体也会被找到。 |
| `Transform` | `YvGetHierarchyPath()` | 获取从场景根节点到 target 的完整路径，例如 Canvas/Panel/Button，常用于日志和排查问题。 |
| `GameObject` | `YvGetHierarchyPath()` | 获取从场景根节点到 target 的完整路径，例如 Canvas/Panel/Button，常用于日志和排查问题。 |
| `Transform` | `YvClearChildren()` | 销毁 target 的全部直接子物体（连同它们的子孙）。 |

### ActiveStateExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Component` | `YvSetActive(active)` | 设置组件所在物体的激活状态（activeSelf），省去 .gameObject。 |
| `GameObject` | `YvToggleActive()` | 切换物体自身的激活状态：已激活则关闭，已关闭则激活。 |
| `Component` | `YvToggleActive()` | 切换组件所在物体的激活状态：已激活则关闭，已关闭则激活。 |
| `Transform` | `YvSetChildrenActive(active)` | 设置全部直接子物体的激活状态，target 自身和孙物体的 activeSelf 不变。 |

### ObjectLifetimeExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `UnityEngine.Object` | `YvDestroy()` | 在运行模式下销毁场景中的对象（GameObject 或组件），销毁在当前帧末生效。 |

### ComponentExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `GameObject` | `YvGetOrAddComponent<T>()` | 获取物体上的 T 组件，没有时自动添加一个。 |
| `GameObject` | `YvGetOrAddComponent<T>(added)` | 获取物体上的 T 组件，没有时自动添加，并通过 added 告知是否新添加，便于只对新组件做初始化。 |
| `GameObject` | `YvGetOrAddComponent(componentType)` | 按运行时类型获取物体上的组件，没有时自动添加，适用于类型在运行时才确定的情况。 |
| `Component` | `YvGetOrAddComponent<T>()` | 获取组件所在物体上的 T 组件，没有时自动添加一个。 |
| `Component` | `YvGetOrAddComponent<T>(added)` | 获取组件所在物体上的 T 组件，没有时自动添加，并通过 added 告知是否新添加。 |
| `Component` | `YvGetOrAddComponent(componentType)` | 按运行时类型获取组件所在物体上的组件，没有时自动添加。 |
| `GameObject` | `YvGetComponentOrThrow<T>()` | 获取必需的 T 组件，缺失时立即抛出带层级路径的异常，便于在初始化时尽早发现配置错误。 |
| `Component` | `YvGetComponentOrThrow<T>()` | 获取组件所在物体上必需的 T 组件，缺失时立即抛出带层级路径的异常。 |
| `GameObject` | `YvTryGetComponentInChildren<T>(component, includeInactive)` | 在 target 自身及全部子孙物体中深度优先查找第一个 T 组件。 |
| `Component` | `YvTryGetComponentInChildren<T>(component, includeInactive)` | 在 target 自身及全部子孙物体中深度优先查找第一个 T 组件。 |
| `GameObject` | `YvTryGetComponentInParent<T>(component, includeInactive)` | 从 target 自身开始逐级向父物体查找第一个 T 组件。 |
| `Component` | `YvTryGetComponentInParent<T>(component, includeInactive)` | 从 target 自身开始逐级向父物体查找第一个 T 组件。 |
| `GameObject` | `YvRemoveComponent<T>(removeAll)` | 在运行模式下移除物体上的 T 组件。 |
| `Component` | `YvRemoveComponent<T>(removeAll)` | 在运行模式下移除物体上的 T 组件。 |
| `GameObject` | `YvAddComponents(componentTypes)` | 按顺序一次添加多个组件。 |
| `Component` | `YvAddComponents(componentTypes)` | 按顺序一次添加多个组件。 |

### BoundsExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Transform` | `YvGetCombinedRendererBounds(includeInactive)` | 计算 target 及全部子孙物体上所有 Renderer 的世界空间合并包围盒（轴对齐）。 |
| `Transform` | `YvTryGetHierarchyLocalBounds(localBounds, includeInactive)` | 计算 target 及全部子孙物体在 target 局部坐标系中的合并包围盒，结果可直接用于 target 上的 BoxCollider。 |

### LayerExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `LayerMask` | `YvContains(layer)` | 判断 LayerMask 是否包含指定 Layer。 |
| `LayerMask` | `YvContains(target)` | 判断 LayerMask 是否包含物体所在的 Layer，常用于在碰撞或射线回调中过滤物体。 |
| `LayerMask` | `YvAddLayer(layer)` | 返回加入指定 Layer 后的新 LayerMask，原值不变。 |
| `LayerMask` | `YvRemoveLayer(layer)` | 返回移除指定 Layer 后的新 LayerMask，原值不变。 |
| `GameObject` | `YvSetLayerRecursive(layer)` | 把物体自身及全部子孙物体（包括未激活的）设置到同一个 Layer。 |

### VectorExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Vector2` | `YvWithX(x)` | 返回把 X 分量替换为 x 的新 Vector2。 |
| `Vector2` | `YvWithY(y)` | 返回把 Y 分量替换为 y 的新 Vector2。 |
| `Vector2` | `YvAddX(x)` | 返回在 X 分量上加上 x 的新 Vector2。 |
| `Vector2` | `YvAddY(y)` | 返回在 Y 分量上加上 y 的新 Vector2。 |
| `Vector3` | `YvWithX(x)` | 返回把 X 分量替换为 x 的新 Vector3。 |
| `Vector3` | `YvWithY(y)` | 返回把 Y 分量替换为 y 的新 Vector3。 |
| `Vector3` | `YvWithZ(z)` | 返回把 Z 分量替换为 z 的新 Vector3。 |
| `Vector3` | `YvAddX(x)` | 返回在 X 分量上加上 x 的新 Vector3。 |
| `Vector3` | `YvAddY(y)` | 返回在 Y 分量上加上 y 的新 Vector3。 |
| `Vector3` | `YvAddZ(z)` | 返回在 Z 分量上加上 z 的新 Vector3。 |

### ColorExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Color` | `YvWithAlpha(alpha)` | 返回只修改透明度的新颜色，原值不变。 |
| `Color` | `YvToHex(includeAlpha)` | 把颜色转换为不带 # 的大写十六进制字符串，例如 FF0000FF。 |
| `string` | `YvTryParseColor(color)` | 把颜色名称或十六进制字符串解析为 Color，十六进制可以省略 #。 |

### GameObjectClickExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `GameObject` | `YvSetClickAction(onClicked)` | 设置物体被鼠标左键点击（或触摸点按）时执行的回调，适用于没有 Button 的 3D 物体或 UI 物体。 |

### RectTransformExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Component` | `YvGetRect()` | 获取组件所在物体的 RectTransform。 |
| `RectTransform` | `YvSetAnchoredPositionX(value)` | 只修改 anchoredPosition 的 x，保留 y。 |
| `RectTransform` | `YvSetAnchoredPositionY(value)` | 只修改 anchoredPosition 的 y，保留 x。 |
| `RectTransform` | `YvSetSizeDeltaX(value)` | 只修改 sizeDelta 的 x，保留 y。 |
| `RectTransform` | `YvSetSizeDeltaY(value)` | 只修改 sizeDelta 的 y，保留 x。 |
| `RectTransform` | `YvSetPivotKeepPosition(pivot)` | 修改 Pivot，同时移动位置补偿，使 UI 在屏幕上的位置和大小保持不变。 |

### GraphicExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Graphic` | `YvSetAlpha(alpha)` | 只修改 Graphic（Image、RawImage、Text 等）颜色的透明度，保留原有 RGB。 |

### CanvasGroupExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `CanvasGroup` | `YvSetCanvasGroupState(alpha, interactable, blocksRaycasts)` | 一次设置 CanvasGroup 的透明度、可交互和射线拦截，常用于整体显示、隐藏或禁用一组 UI。 |

### SelectableExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Selectable` | `YvSetNavigationNone()` | 把 Selectable（Button、Toggle、InputField 等）的导航模式设为 None，方向键和手柄无法再移动焦点到它或从它移出。 |

### LayoutExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `RectTransform` | `YvRebuildLayoutTree()` | 立即重建以 root 为根的整个布局树（先子节点、后父节点），调用后即可读取正确的尺寸和位置。 |
| `RectTransform` | `YvRebuildNearestLayoutNextFrame(coroutineRunner)` | 下一帧重建 target 自身或向上最近的 LayoutGroup / ContentSizeFitter 节点。 |

### UIEventExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Button` | `YvSetOnClick(callback, removeAllListeners)` | 设置 Button 的点击事件 onClick。 |
| `Toggle` | `YvSetOnValueChanged(callback, removeAllListeners)` | 设置 Toggle 的开关变化事件 onValueChanged。 |
| `Slider` | `YvSetOnValueChanged(callback, removeAllListeners)` | 设置 Slider 的数值变化事件 onValueChanged。 |
| `Scrollbar` | `YvSetOnValueChanged(callback, removeAllListeners)` | 设置 Scrollbar 的数值变化事件 onValueChanged。 |
| `Dropdown` | `YvSetOnValueChanged(callback, removeAllListeners)` | 设置 Dropdown 的选项变化事件 onValueChanged。 |
| `ScrollRect` | `YvSetOnValueChanged(callback, removeAllListeners)` | 设置 ScrollRect 的滚动位置变化事件 onValueChanged。 |
| `InputField` | `YvSetOnValueChanged(callback, removeAllListeners)` | 设置 InputField 的文本变化事件 onValueChanged。 |
| `InputField` | `YvSetOnEndEdit(callback, removeAllListeners)` | 设置 InputField 的编辑结束事件 onEndEdit。 |
| `InputField` | `YvSetOnSubmit(callback, removeAllListeners)` | 设置 InputField 的提交事件 onSubmit（按回车提交时触发）。 |
| `MaskableGraphic` | `YvSetOnCullStateChanged(callback, removeAllListeners)` | 设置 MaskableGraphic（Image、Text 等）的裁剪状态变化事件 onCullStateChanged。 |
| `EventTrigger` | `YvSetEventTrigger(eventType, callback, removeAllListeners)` | 设置 EventTrigger 上指定类型事件的监听；该类型条目不存在时自动创建。 |
| `TMP_Dropdown` | `YvSetOnValueChanged(callback, removeAllListeners)` | 设置 TMP_Dropdown 的选项变化事件 onValueChanged。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |
| `TMP_InputField` | `YvSetOnValueChanged(callback, removeAllListeners)` | 设置 TMP_InputField 的文本变化事件 onValueChanged。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |
| `TMP_InputField` | `YvSetOnEndEdit(callback, removeAllListeners)` | 设置 TMP_InputField 的编辑结束事件 onEndEdit。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |
| `TMP_InputField` | `YvSetOnSubmit(callback, removeAllListeners)` | 设置 TMP_InputField 的提交事件 onSubmit（按回车提交时触发）。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |
| `TMP_InputField` | `YvSetOnSelect(callback, removeAllListeners)` | 设置 TMP_InputField 的获得焦点事件 onSelect。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |
| `TMP_InputField` | `YvSetOnDeselect(callback, removeAllListeners)` | 设置 TMP_InputField 的失去焦点事件 onDeselect。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |
| `TMP_InputField` | `YvSetOnTextSelection(callback, removeAllListeners)` | 设置 TMP_InputField 的选区变化事件 onTextSelection。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |
| `TMP_InputField` | `YvSetOnEndTextSelection(callback, removeAllListeners)` | 设置 TMP_InputField 的选区结束事件 onEndTextSelection。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |
| `TMP_InputField` | `YvSetOnTouchScreenKeyboardStatusChanged(callback, removeAllListeners)` | 设置 TMP_InputField 的触屏键盘状态变化事件。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |

### GameObjectCompositeExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `GameObject` | `YvAddFittedBoxCollider(includeInactive)` | 在物体上添加或更新一个 BoxCollider，使其正好包住物体及全部子孙的模型和子碰撞体。 |
| `Component` | `YvAddFittedBoxCollider(includeInactive)` | 在组件所在物体上添加或更新一个 BoxCollider，使其正好包住物体及全部子孙的模型和子碰撞体。 |

### TransformCompositeExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Transform` | `YvSetParentAndResetLocal(parent)` | 把 target 设为 parent 的子物体，并把本地位置、旋转、缩放重置为默认值，常用于把生成的物体挂到挂点上。 |

### TextCompositeExtensions

| 扩展对象 | 方法 | 说明 |
| --- | --- | --- |
| `Text` | `YvSetText(value, hideWhenEmpty)` | 设置 UGUI Text 文本，并可根据内容是否为空控制物体显隐。 |
| `TMP_Text` | `YvSetText(value, hideWhenEmpty)` | 设置 TMP_Text 文本，并可根据内容是否为空控制物体显隐。仅在项目包含 TextMesh Pro 时可用。（需要 TextMesh Pro） |

## 环境要求

- Unity 2021.3 或更高版本
- Unity 3D Physics 与 Unity UI（通过包依赖自动引入）
- TextMesh Pro（可选，存在时自动启用 TMP 相关扩展）
- Highlight Plus，仅在导入对应 Sample 时需要

## 许可证

本包使用 MIT License，详情参见仓库中的许可证文件。

## 依赖说明

本包不依赖 Inspector Groups 或其他 Yvsim 包；Samples 中的可选组合需按示例说明准备依赖。

## 从 GitHub 安装

仓库：[com.yvsim.extensions](https://github.com/liuyan7669/com.yvsim.extensions)。仓库名与 UPM 包名一致。

在 Unity Package Manager 选择 Add package from git URL，输入：

```text
https://github.com/liuyan7669/com.yvsim.extensions.git#main
```

此地址跟随 main 分支。需要固定 1.0.0 源码时，将 `#main` 改为 `#v1.0.0`；Unity 官方依赖仍按 package.json 解析。

## 从 OpenUPM 安装

在 Unity 的 Project Settings > Package Manager 中添加 Scoped Registry：

- Name：OpenUPM
- URL：`https://package.openupm.com`
- Scope：`com.yvsim`

然后在 Package Manager 中通过 Add package by name 输入 `com.yvsim.extensions`，版本填 `1.0.0`。也可从 My Registries 中选择本包。声明的 Yvsim 依赖由 OpenUPM 解析，Unity 官方依赖仍由 Unity 注册表提供。

如果已安装 OpenUPM CLI，可在工程根目录执行：

```sh
openupm add com.yvsim.extensions@1.0.0
```

[OpenUPM 包页面](https://openupm.com/packages/com.yvsim.extensions/)
