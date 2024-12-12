mergeInto(LibraryManager.library, {
  isLoadedF: function () {
    window.dispatchReactUnityEvent("isLoadedF");
  },
});