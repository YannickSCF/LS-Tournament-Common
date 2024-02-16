/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     14/02/2024
 **/

// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Custom dependencies
using YannickSCF.LSTournaments.Common.Views.PouleTable.Design.Objects;
using YannickSCF.LSTournaments.Common.Views.PouleTable.Objects;

namespace YannickSCF.LSTournaments.Common.Views.PouleTable.Design {
    [RequireComponent(typeof(PouleTableView))]
    public class PouleTableViewEditor : MonoBehaviour {


        [Header("----------------------\nPoule Main Objects\n----------------------")]
        [SerializeField] private RectTransform _completePoulePanel;
        [SerializeField, Range(3, 12)] private int _activeAthletes = 4;

        [Space(15)]
        [SerializeField] private RectTransform _titlePanel;
        [SerializeField, Range(200, 500)] private int _titlePanelWidth;
        [SerializeField, Range(25, 75)] private int _titlePanelHeight;
        [Space(15)]
        [SerializeField] private RectTransform _tablePanel;

        [Header("----------------------\nPoule Table Header Objects\n----------------------")]
        [SerializeField] private RectTransform _tableHeaderPanel;
        [SerializeField] private RectTransform _tableHeaderMainColumn;
        [SerializeField] private RectTransform _tableHeaderOtherColumns;
        [SerializeField, Range(25, 75)] private int _tableHeaderHeight;
        [SerializeField] private List<PouleDataHeader> _allPouleHeaders;

        [Header("----------------------\nPoule Table Body Objects\n----------------------")]
        [SerializeField] private RectTransform _tableBodyPanel;
        [SerializeField] private List<PouleAthleteView> _allPouleRows;
        [SerializeField, Range(25, 75)] private int _tableBodyRowHeight;
        [SerializeField, Range(200, 500)] private int _mainColumnWidth;

        [Header("----------------------\nAll Poule Borders\n----------------------")]
        [Header("External Borders")]
        [SerializeField] private List<PouleBorder> _externalBorders;
        [SerializeField, Range(0f, 10f)] private int _externalBordersSize;
        [SerializeField] private Color _externalBordersColor;

        [Space(15), Header("Internal Borders")]
        [SerializeField] private List<PouleBorder> _rowsBorders;
        [SerializeField] private List<PouleBorder> _colsBorders;
        [Header(" -> Main Internal Parameters")]
        [SerializeField, Range(0f, 10f)] private int _mainInternalBordersSize;
        [SerializeField] private Color _mainInternalBordersColor;
        [Header(" -> Other Internal Parameters")]
        [SerializeField, Range(0f, 10f)] private int _internalBordersSize;
        [SerializeField] private Color _internalBordersColor;

#if UNITY_EDITOR
        private void OnValidate() {
            SetNumberOfAthletes(_activeAthletes);

            SetTitlePanelSize(_titlePanelWidth, _titlePanelHeight);

            SetHeaderHeight(_tableHeaderHeight);

            if (_tableHeaderMainColumn != null && _tableHeaderOtherColumns != null) {
                SetMainColumnWidth(_mainColumnWidth);
            }
            SetTableBodyRowHeight(_tableBodyRowHeight);

            SetBorderSize(PouleBorder.BorderType.External, _externalBordersSize);
            SetBorderSize(PouleBorder.BorderType.MainInternal, _mainInternalBordersSize);
            SetBorderSize(PouleBorder.BorderType.Internal, _internalBordersSize);

            SetBorderColor(PouleBorder.BorderType.External, _externalBordersColor);
            SetBorderColor(PouleBorder.BorderType.MainInternal, _mainInternalBordersColor);
            SetBorderColor(PouleBorder.BorderType.Internal, _internalBordersColor);
        }
#endif

        public void SetNumberOfAthletes(int numberOfAthletes) {
            _activeAthletes = numberOfAthletes;

            List<int> fixedColumnsIndexes = new List<int>();
            for (int i = 0; i < _allPouleHeaders.Count; ++i) {
                if (_allPouleHeaders[i].FixedColumn) {
                    fixedColumnsIndexes.Add(i);
                }
            }

            // Set in each row every column active and deactive all correspondant rows
            for (int i = 0; i < _allPouleRows.Count; ++i) {
                _allPouleRows[i].SetColumnsActive(_activeAthletes, fixedColumnsIndexes);
                _allPouleRows[i].gameObject.SetActive(i < _activeAthletes);
            }

            // Deactive header columns unneeded
            int rowsRelatedCount = 0;
            for (int i = 0; i < _allPouleHeaders.Count; ++i) {
                if (_allPouleHeaders[i].FixedColumn) {
                    _allPouleHeaders[i].SetActive(true);
                } else {
                    _allPouleHeaders[i].SetActive(rowsRelatedCount < _activeAthletes);
                    ++rowsRelatedCount;
                }
            }

            UpdateAllDataHeaderBoxes();
            UpdateContentPouleView();
        }

        public void SetTitlePanelSize(int newWidth, int newHeight) {
            // HEIGHT
            _titlePanelHeight = newHeight;

            // Set height panel size
            _titlePanel.sizeDelta = new Vector2(_titlePanel.sizeDelta.x, _titlePanelHeight);
            // Relocate table panel according to new title panel height
            _tablePanel.offsetMax = new Vector2(0, -_titlePanelHeight);

            // WIDTH
            _titlePanelWidth = newWidth;
            // Avoid to have a title panel wider than the column of the athlete names
            if (_titlePanelWidth >= _mainColumnWidth) {
                _titlePanel.sizeDelta = new Vector2(_mainColumnWidth, _titlePanel.sizeDelta.y);
            } else {
                _titlePanel.sizeDelta = new Vector2(_titlePanelWidth, _titlePanel.sizeDelta.y);
            }

            // Update the poule total size to adapt last changes
            UpdatePouleSize();
        }

        public void SetHeaderHeight(int newHeight) {
            _tableHeaderHeight = newHeight;

            // Set header height size
            _tableHeaderPanel.sizeDelta = new Vector2(_tableHeaderPanel.sizeDelta.x, newHeight);
            // Set body top parameter
            _tableBodyPanel.offsetMax = new Vector2(_tableBodyPanel.offsetMax.x, -_tableHeaderHeight);

            // Update the poule total size to adapt last changes
            UpdatePouleSize();
        }

        public void SetMainColumnWidth(int newWidth) {
            _mainColumnWidth = newWidth;

            _tableHeaderMainColumn.sizeDelta = new Vector2(newWidth, _tableHeaderMainColumn.sizeDelta.y);
            _tableHeaderOtherColumns.offsetMin = new Vector2(newWidth, _tableHeaderOtherColumns.offsetMin.y);

            foreach (PouleAthleteView row in _allPouleRows) {
                row.SetNameColumnWidth(newWidth);
            }

            if (_mainColumnWidth <= _titlePanel.sizeDelta.x ||
                _titlePanel.sizeDelta.x < _titlePanelWidth) {
                _titlePanel.sizeDelta = new Vector2(_mainColumnWidth, _titlePanel.sizeDelta.y);
            }

            // Update the poule total size to adapt last changes
            UpdatePouleSize();
        }

        public void SetTableBodyRowHeight(int newHeight) {
            _tableBodyRowHeight = newHeight;

            // Modify all rows with new height
            foreach (PouleAthleteView row in _allPouleRows) {
                row.SetRowHeight(_tableBodyRowHeight);
            }
            // Update vertical positions of all rows
            UpdateVerticalPositions();

            // To modify data boxes width and make them a square
            UpdateAllDataHeaderBoxes();
            UpdateContentPouleView();
        }

        #region Methods to manage Borders
        public void SetBorderSize(PouleBorder.BorderType borderType, int newSize) {
            switch (borderType) {
                case PouleBorder.BorderType.MainInternal: _mainInternalBordersSize = newSize; break;
                case PouleBorder.BorderType.External: _externalBordersSize = newSize; break;
                case PouleBorder.BorderType.Internal:
                default: _internalBordersSize = newSize; break;
            }

            if (borderType == PouleBorder.BorderType.External) {
                foreach (PouleBorder extBorder in _externalBorders) {
                    extBorder.SetBorderWidth(_externalBordersSize);
                }
            } else {
                _rowsBorders = new List<PouleBorder>(_allPouleRows.Select(x => x.Border).ToList());
                foreach (PouleBorder rowBorder in _rowsBorders) {
                    if (rowBorder.Type == borderType) {
                        rowBorder.SetBorderWidth(borderType == PouleBorder.BorderType.MainInternal ?
                            _mainInternalBordersSize : _internalBordersSize);
                    }
                }

                _colsBorders = new List<PouleBorder>(_allPouleHeaders.Select(x => x.Border).ToList());
                foreach (PouleBorder colBorder in _colsBorders) {
                    if (colBorder.Type == borderType) {
                        colBorder.SetBorderWidth(borderType == PouleBorder.BorderType.MainInternal ?
                            _mainInternalBordersSize : _internalBordersSize);
                    }
                }

                UpdateAllDataHeaderBoxes();
                UpdateVerticalPositions();
                UpdateContentPouleView();
            }
        }

        public void SetBorderColor(PouleBorder.BorderType borderType, Color newColor) {
            switch (borderType) {
                case PouleBorder.BorderType.MainInternal: _mainInternalBordersColor = newColor; break;
                case PouleBorder.BorderType.External: _externalBordersColor = newColor; break;
                case PouleBorder.BorderType.Internal:
                default: _internalBordersColor = newColor; break;
            }

            if (borderType == PouleBorder.BorderType.External) {
                foreach (PouleBorder extBorder in _externalBorders) {
                    extBorder.SetColor(_externalBordersColor);
                }
            } else {
                _rowsBorders = new List<PouleBorder>(_allPouleRows.Select(x => x.Border).ToList());
                foreach (PouleBorder rowBorder in _rowsBorders) {
                    if (rowBorder.Type == borderType) {
                        rowBorder.SetColor(borderType == PouleBorder.BorderType.MainInternal ?
                            _mainInternalBordersColor : _internalBordersColor);
                    }
                }

                _colsBorders = new List<PouleBorder>(_allPouleHeaders.Select(x => x.Border).ToList());
                foreach (PouleBorder colBorder in _colsBorders) {
                    if (colBorder.Type == borderType) {
                        colBorder.SetColor(borderType == PouleBorder.BorderType.MainInternal ?
                         _mainInternalBordersColor : _internalBordersColor);
                    }
                }
            }
        }
        
        private int GetBorderWidthByType(PouleBorder.BorderType borderType) {
            switch (borderType) {
                case PouleBorder.BorderType.MainInternal: return _mainInternalBordersSize;
                case PouleBorder.BorderType.External: return _externalBordersSize;
                case PouleBorder.BorderType.Internal:
                default: return _internalBordersSize;
            }
        }
        #endregion

        private void UpdateAllDataHeaderBoxes() {
            int xPosition = 0;
            foreach (PouleDataHeader headerBox in _allPouleHeaders) {
                if (headerBox.activeSelf) {
                    int boxWidth = headerBox.ColumnOfSquares ?
                        _tableBodyRowHeight : headerBox.CustomColumnSize;

                    xPosition += GetBorderWidthByType(headerBox.Border.Type);

                    headerBox.SetWidth(boxWidth);
                    headerBox.SetXPosition(xPosition);

                    xPosition += boxWidth;
                }
            }
        }
        private void UpdateVerticalPositions() {
            int yPosition = 0;
            foreach (PouleAthleteView row in _allPouleRows) {
                yPosition += GetBorderWidthByType(row.Border.Type);
                row.SetYPosition(-yPosition);
                yPosition += _tableBodyRowHeight;
            }
        }
        private void UpdateContentPouleView() {
            // Update rows data boxes sizes and x position
            UpdateAllDataBoxes();
            // Update column borders position and Y axis size
            UpdateColumnBorders();
            // Update general poule size to fit content inside
            UpdatePouleSize();
        }

        private void UpdateAllDataBoxes() {
            int xPosition = 0;
            for (int i = 0; i < _allPouleHeaders.Count; ++i) {
                if (_allPouleHeaders[i].activeSelf) {
                    int boxWidth = _allPouleHeaders[i].ColumnOfSquares ?
                        _tableBodyRowHeight : _allPouleHeaders[i].CustomColumnSize;

                    xPosition += GetBorderWidthByType(_allPouleHeaders[i].Border.Type);

                    foreach (PouleAthleteView row in _allPouleRows) {
                        row.SetWidthAndXOfDataBox(i, boxWidth, xPosition);
                    }

                    xPosition += boxWidth;
                }
            }
        }
        private void UpdatePouleSize() {
            // Calculate active rows for height
            int activeRows = _allPouleRows.Count(x => x.gameObject.activeSelf);
            // Calculate the sum of widths for width
            int totalBoxesWidth = 0;
            for (int i = 0; i < _allPouleHeaders.Count; ++i) {
                if (_allPouleHeaders[i].activeSelf) {
                    totalBoxesWidth += _allPouleHeaders[i].ColumnOfSquares ?
                        _tableBodyRowHeight : _allPouleHeaders[i].CustomColumnSize;
                }
            }

            // Get width of all internal borders
            int totalBorderWidth = 0;
            foreach (PouleDataHeader headerBox in _allPouleHeaders) {
                if (headerBox.activeSelf) {
                    totalBorderWidth += GetBorderWidthByType(headerBox.Border.Type);
                }
            }
            // Get height of all internal borders
            int totalBorderHeight = 0;
            foreach (PouleAthleteView row in _allPouleRows) {
                if (row.activeSelf) {
                    totalBorderHeight += GetBorderWidthByType(row.Border.Type);
                }
            }

            // Set Width and height of complete Poule Panel
            _completePoulePanel.sizeDelta = new Vector2(
                _mainColumnWidth + totalBorderWidth + totalBoxesWidth,
                _titlePanelHeight + _tableHeaderHeight + totalBorderHeight + _tableBodyRowHeight * activeRows);
        }
        private void UpdateColumnBorders() {
            // Calculate active rows for height
            int activeRows = _allPouleRows.Count(x => x.gameObject.activeSelf);

            int totalBorderHeight = 0;
            foreach (PouleAthleteView row in _allPouleRows) {
                if (row.activeSelf) {
                    totalBorderHeight += GetBorderWidthByType(row.Border.Type);
                }
            }

            _colsBorders = new List<PouleBorder>(_allPouleHeaders.Select(x => x.Border).ToList());
            foreach (PouleBorder colBorder in _colsBorders) {
                colBorder.LengthenDown(totalBorderHeight + _tableBodyRowHeight * activeRows);
            }
        }

    }
}
