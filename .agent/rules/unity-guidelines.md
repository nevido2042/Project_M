---
trigger: always_on
---

# 유니티 개발 가이드라인 (Unity Development Guidelines)

## 1. 코드 스타일 및 구조 (Code Style)
- **캡슐화 원칙:** 외부 스크립트에서 직접 접근해야 하는 경우가 아니라면, 인스펙터(Inspector) 창에 노출할 변수는 단순 `public` 필드 대신 반드시 `[SerializeField] private`을 사용한다.
- **프로퍼티 활용:** 외부에서 변수 값을 읽거나 써야 할 때는 단순히 `public` 변수를 열어두지 않고, 데이터 보호를 위해 C#의 프로퍼티(Properties - `get`, `set`)를 적극적으로 활용한다. (예: `public int Health { get; private set; }`)
- **명명 규칙:** 일반 변수명은 카멜 케이스(camelCase)를 사용하고, 클래스, 메서드, 프로퍼티명은 파스칼 케이스(PascalCase)를 엄격하게 지킨다.
- **언어 규칙:** 구현 계획서나 작업 설명은 반드시 한국어로 작성한다.