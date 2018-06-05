import React, { Component } from 'react';
import TestResultRow from './TestResultRow'
import HomeLink from '../common/HomeLink'
import GroupSuitesButton from './GroupSuitesButton'

class LatestTestResultsList extends Component {

  render() {
    const grouped = this.props.grouped;
    return (
      <div className="container">
        <h4><HomeLink grouped={grouped}/></h4>
        <div className="row">
          <div className="col-4"/>
          <div className="col-4">
            <h2 className="text-center">{this.props.suiteName}</h2>
          </div>
          <div className="col-4">
            <GroupSuitesButton grouped={grouped} suiteId={this.props.suiteId}/>
          </div>
        </div>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Name</th>
              {grouped && <th scope="col">Type</th>}
              <th scope="col">Status</th>
              <th scope="col">History</th>
            </tr>
          </thead>
          <tbody>
            {this.props.testResults.map((testResult) =>
              <TestResultRow key={testResult.TestResultID} testResult={testResult} grouped={grouped} />
            )}
          </tbody>
        </table>
      </div>
    );
  }
}

export default LatestTestResultsList;
