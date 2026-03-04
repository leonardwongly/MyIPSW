# Manual QA - Unified Firmware Finder

## Scope
This checklist validates the unified flow on `Default.aspx`, compatibility forwarding from `Link.aspx`, telemetry safety, and recoverable states.

## Preconditions
1. Deploy build with `TelemetryEnabled=true`.
2. Clear browser cache/cookies.
3. Test on desktop and mobile viewport.

## 1) Entry points and navigation
1. Open `Default.aspx` directly.
2. Confirm navbar shows `Find Firmware` as active.
3. Open `Link.aspx` directly.
4. Confirm browser redirects to `Default.aspx?result=links`.
5. If query params are supplied on `Link.aspx` (`source`, `device`, `version`), confirm only safe values are forwarded.

## 2) Source and target selection flow
1. In `Default.aspx`, choose `Official IPSW`.
2. Confirm only `iPhone/iPad/iPod/Mac` selectors are visible.
3. Select an iPhone model and verify other device dropdowns reset to default.
4. Switch source to `OTA`.
5. Confirm OTA-eligible selectors are visible (`Watch`, `Apple TV`, `HomePod`) and previous selection is cleared.
6. Switch source to `Version` and then `Version (OTA)`.
7. Confirm only one version dropdown is visible for each mode.

## 3) Output mode behavior
1. Select valid source + target and choose `Table View`.
2. Click `Retrieve` and verify results render in table format.
3. Switch to `Links View` and confirm links render instead of table.
4. Ensure `Open All Links` is visible only in links mode with non-empty results.

## 4) Download-all guardrails
1. In links mode with results, click `Open All Links`.
2. Confirm confirmation dialog shows expected count behavior.
3. Cancel action and ensure no new tabs open.
4. Confirm action and verify tabs open.
5. With popups blocked, confirm warning appears with recovery guidance.

## 5) Error and recovery states
1. Simulate upstream failure (temporary network block to `api.ipsw.me`).
2. Click `Retrieve`.
3. Confirm inline danger status appears and `Retry` button is visible.
4. Restore connectivity and click `Retry`.
5. Confirm successful results render without full-page error redirect.

## 6) Query-string hydration
1. Open `Default.aspx?source=official&device=<valid-id>&result=table`.
2. Confirm source and target are pre-selected and retrieval runs automatically.
3. Open with invalid params (unknown source/device/version).
4. Confirm invalid params are ignored safely and warning text is shown inline.

## 7) Accessibility and responsiveness
1. Navigate entire flow using keyboard only (`Tab`, `Shift+Tab`, `Enter`, arrow keys for radios).
2. Confirm visible focus indicators on all interactive controls.
3. Verify status messages are readable and controls remain reachable on mobile widths (<= 390px).
4. Check contrast for primary action, warnings, and success/error states.

## 8) Telemetry endpoint safety checks
1. Verify normal interactions send POST requests to `Telemetry.ashx` with 204 responses.
2. Send malformed JSON payload to `Telemetry.ashx` and verify 400.
3. Send payload with unknown fields and verify 400.
4. Send oversized payload (>2KB) and verify 400.
5. Burst many events with same `sessionId` and verify eventual 429 responses.

## Exit criteria
- All sections pass on desktop and mobile viewport.
- No unhandled exceptions in browser console for core flow.
- No regressions in source/target retrieval behavior for Official, OTA, Version, and Version (OTA).
