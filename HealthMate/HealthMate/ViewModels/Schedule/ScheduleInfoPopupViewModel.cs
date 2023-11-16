﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Services;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class ScheduleInfoPopupViewModel : BaseViewModel
{
    private readonly PopupService _popupService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    public int closeBtnColSpan;
    [ObservableProperty]
    public bool isMedsTakenBtnVisible;
    [ObservableProperty]
    private ScheduleTable passedSchedule;

    public ScheduleInfoPopupViewModel(PopupService popupService, RealmService realmService)
    {
        _popupService = popupService;
        _realmService = realmService;
    }

    [RelayCommand]
    public async Task ClosePopup()
    {
        await _popupService.ClosePopup();
    }

    [RelayCommand]
    public async Task MedsTaken()
    {
        await _realmService.Write(() => PassedSchedule.ScheduleState = (int)ScheduleState.Taken);
        await ClosePopup();
    }

    public override void OnNavigatedTo()
    {
        CloseBtnColSpan = (ScheduleState)PassedSchedule.ScheduleState == ScheduleState.Taken ? 2 : 1;
        IsMedsTakenBtnVisible = (ScheduleState)PassedSchedule.ScheduleState == ScheduleState.Taken;
    }
}