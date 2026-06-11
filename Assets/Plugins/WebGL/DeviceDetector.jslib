mergeInto(LibraryManager.library, {
  IsRealMobileDevice: function() {
    var userAgent = navigator.userAgent;
    
    // Проверка на телефон или старый iPad/Android
    var isMobileDevice = (
      /\b(BlackBerry|webOS|iPhone|IEMobile)\b/i.test(userAgent) ||
      /\b(Android|Windows Phone|iPad|iPod)\b/i.test(userAgent)
    );
    
    // СПЕЦИАЛЬНО ДЛЯ НОВЫХ IPAD (на iOS 13+ UserAgent = "Mac")
    var isNewIPad = (
      (userAgent.indexOf("Mac") !== -1) &&  // includes() может не работать в старых браузерах
      (typeof document.ontouchend !== 'undefined') &&
      (navigator.maxTouchPoints > 0)
    );
    
    // Доп. проверка на реальный тач (эмуляция в Chrome DevTools)
    var hasTouchScreen = ('ontouchstart' in window) || (navigator.maxTouchPoints > 0);
    
    // Если есть тач, НО десктопный User Agent без Mac — возможно эмуляция
    var isDesktopEmulation = (
      hasTouchScreen && 
      !isMobileDevice && 
      !isNewIPad &&
      (userAgent.indexOf("Windows") !== -1 || userAgent.indexOf("Linux") !== -1)
    );
    
    // Логируем в консоль браузера (для отладки)
    console.log("[Unity] Device detection:", {
      userAgent: userAgent,
      isMobileDevice: isMobileDevice,
      isNewIPad: isNewIPad,
      hasTouchScreen: hasTouchScreen,
      isDesktopEmulation: isDesktopEmulation,
      result: (isMobileDevice || isNewIPad) && !isDesktopEmulation
    });
    
    return (isMobileDevice || isNewIPad) && !isDesktopEmulation;
  }
});