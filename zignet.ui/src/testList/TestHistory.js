import React, { Component } from 'react';
import moment from 'moment';
import { getErrorDivAttributes, getDivTitle } from '../common/HistoryBarProvider'
import './css/testHistory.css';

class TestHistory extends Component {
  constructor() {
    super();
    this.state = {
      failureDivs: []
    };
  }

  _getFailureDivs(testFailureDurations) {
    let historyBarWidth = this.historyBarDiv.offsetWidth
    let now = moment();
    var failureDivs = [];
    for (var i = 0; i < testFailureDurations.length; i++) {
      let failureDivAttributes = getErrorDivAttributes(historyBarWidth, now, testFailureDurations[i]);
      failureDivs.push(<div key={i} style={failureDivAttributes} title={getDivTitle(testFailureDurations[i])}/>);
    }
    return failureDivs;
  }

  componentDidMount() {
    this.setState({
      failureDivs: this._getFailureDivs(this.props.testFailureDurations)
    });
  }

  componentWillReceiveProps(props) {
    this.setState({
      failureDivs: this._getFailureDivs(props.testFailureDurations)
    });
  }

  render() {
    return (
      <div className="testHistoryBar" ref={(div) => this.historyBarDiv = div}>
        {this.state.failureDivs.map((failureDiv) => failureDiv)}      
      </div>
    );
  }
}

export default TestHistory;